using System.Diagnostics;
using System.Numerics;

namespace System.Linq;

public readonly partial record struct RangeEnumerable : ICollection<int>
{
#if NETCOREAPP3_1 || NET
    // Up to 512bit-wide according to upcoming AVX512 support in .NET 8
    private static readonly Vector<int> IncrementMask = new(
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
        InitializeSpan(_start, _end, span);
        return;
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
    private void InitializeSpan(int start, int end, Span<int> destination)
    {
        Debug.Assert(start != end);
        Debug.Assert(destination.Length != 0 && destination.Length <= Count);

        if (destination.Length < Vector<int>.Count * 2)
        {
            // The caller *must* guarantee that destination length can fit the range
            ref var pos = ref destination[0];
            foreach (var num in this)
            {
                pos = num;
                pos = ref Unsafe.Add(ref pos, 1);
            }
        }
        else
        {
            InitializeSpanCore(start, end, destination);
        }
    }

    // TODO: Rewrite to pure ref/pointer arithmetics and indexing
    private void InitializeSpanCore(int start, int end, Span<int> destination)
    {
        (int shift, start) = start < end
            ? (1, start)
            : (-1, start - 1);

        var mask = IncrementMask * shift;
        var width = Vector<int>.Count;
        var stride = Vector<int>.Count * 2;
        var remainder = destination.Length % stride;

        ref var pos = ref destination[0];
        for (var i = 0; i < destination.Length - remainder; i += stride)
        {
            var num = start + (i * shift);
            var num2 = start + ((i + width) * shift);

            var value = new Vector<int>(num) + mask;
            var value2 = new Vector<int>(num2) + mask;

            ref var dest = ref Unsafe.Add(ref pos, i);
            ref var dest2 = ref Unsafe.Add(ref dest, width);

            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref dest), value);
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref dest2), value2);
        }

        for (var i = destination.Length - remainder; i < destination.Length; i++)
        {
            Unsafe.Add(ref pos, i) = start + (i * shift);
        }
    }
#endif
}
