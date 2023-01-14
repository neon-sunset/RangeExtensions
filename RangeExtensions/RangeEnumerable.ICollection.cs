using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace System.Linq;

public readonly partial record struct RangeEnumerable : ICollection<int>
{
#if NETCOREAPP3_1 || NET
    // Up to 512bit-wide according to upcoming AVX512 support in .NET 8
    private static readonly Vector<int> InitMask = new(
        (ReadOnlySpan<int>)new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
#endif

    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _start < _end
            ? _end - _start
            : _start - _end;
    }

    public bool IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(int item)
    {
        if (Count is 0)
        {
            return false;
        }

        var (min, max) = MinAndMaxUnchecked();

        return item >= min && item <= max;
    }

    public void CopyTo(int[] array, int index)
    {
#if !NETSTANDARD2_0
        CopyTo(array.AsSpan(), index);
#else
        if (index < 0 || index > array.Length)
        {
            ThrowHelpers.ArgumentOutOfRange();
        }

        var count = Count;
        if (count > (array.Length - index))
        {
            ThrowHelpers.ArgumentException();
        }

        if (array.Length is 0 || count is 0)
        {
            return;
        }

        var i = index;
        foreach (var num in this)
        {
            array[i] = num;
            i++;
        }
#endif
    }

#if !NETSTANDARD2_0
    public void CopyTo(Span<int> span, int index)
    {
        if (index < 0 || index > span.Length)
        {
            ThrowHelpers.ArgumentOutOfRange();
        }

        var count = Count;
        if (count > (span.Length - index))
        {
            ThrowHelpers.ArgumentException();
        }

        if ((span = span.Slice(index, count)).Length is 0)
        {
            return;
        }

#if NETCOREAPP3_1 || NET
        InitializeSpan(span);
#else
        var enumerator = GetEnumerator();
        for (var i = 0; i < span.Length; i++)
        {
            enumerator.MoveNext();
            span[i] = enumerator.Current;
        }
#endif
    }
#endif

    public void Add(int item)
    {
        throw new NotSupportedException();
    }

    public void Clear()
    {
        throw new NotSupportedException();
    }

    public bool Remove(int item)
    {
        throw new NotSupportedException();
    }

#if NETCOREAPP3_1 || NET
    /// <summary>
    /// Contract: destination length must be greater than 0 and equal to .Count
    /// </summary>
    private void InitializeSpan(Span<int> destination)
    {
        Debug.Assert(_start != _end);
        Debug.Assert(destination.Length != 0 && destination.Length == Count);

        if (destination.Length < Vector<int>.Count * 2)
        {
            // The caller *must* guarantee that destination length can fit the range
            ref var pos = ref MemoryMarshal.GetReference(destination);
            foreach (var num in this)
            {
                pos = num;
                pos = ref Unsafe.Add(ref pos, 1);
            }
        }
        else
        {
            InitializeSpanCore(destination);
        }
    }

    private void InitializeSpanCore(Span<int> destination)
    {
        var (direction, start) = _start < _end
            ? (1, _start)
            : (-1, _start - 1);

        var width = Vector<int>.Count;
        var stride = Vector<int>.Count * 2;
        var remainder = destination.Length % stride;

        var mask = new Vector<int>(stride) * direction;
        var value = new Vector<int>(start) + (InitMask * direction);
        var value2 = value + (new Vector<int>(width) * direction);

        ref var pos = ref MemoryMarshal.GetReference(destination);
        ref var limit = ref Unsafe.Add(ref pos, destination.Length - remainder);
        while (!Unsafe.AreSame(ref pos, ref limit))
        {
            Unsafe.WriteUnaligned(ref ByteRef(ref pos), value);
            Unsafe.WriteUnaligned(ref ByteRef(ref Unsafe.Add(ref pos, width)), value2);

            value += mask;
            value2 += mask;
            pos = ref Unsafe.Add(ref pos, stride);
        }

        var num = start + ((destination.Length - remainder) * direction);
        limit = ref Unsafe.Add(ref limit, remainder);
        while (!Unsafe.AreSame(ref pos, ref limit))
        {
            pos = num;
            pos = ref Unsafe.Add(ref pos, 1);
            num += direction;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref byte ByteRef<T>(ref T source) => ref Unsafe.As<T, byte>(ref source);
#endif
}
