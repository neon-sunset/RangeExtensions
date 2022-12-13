using System.Collections;
using System.Runtime.InteropServices;

namespace System.Linq;

[StructLayout(LayoutKind.Auto)]
public readonly partial record struct RangeEnumerable
{
    private readonly int _start;
    private readonly int _end;

    public static RangeEnumerable Empty => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable(Range range)
    {
        var (start, end) = range.UnwrapUnchecked();

        ThrowHelpers.CheckInvalid(start, end);

        _start = start;
        _end = end;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal RangeEnumerable(int start, int end)
    {
        _start = start;
        _end = end;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator()
    {
        return GetEnumeratorUnchecked();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Enumerator GetEnumeratorUnchecked()
    {
        return new Enumerator(_start, _end);
    }

    [StructLayout(LayoutKind.Auto)]
    public record struct Enumerator : IEnumerator<int>
    {
        private readonly int _shift;
        private readonly int _end;

        private int _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(int start, int end)
        {
            if (start < end)
            {
                _shift = 1;
                _current = start - 1;
                _end = end;
            }
            else
            {
                _shift = -1;
                _current = start;
                _end = end - 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return (_current += _shift) != _end;
        }

        public int Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _current;
        }

        object IEnumerator.Current => _current;

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose() { }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator RangeEnumerable(Range range) => new(range);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Range(RangeEnumerable enumerable) => enumerable._start..enumerable._end;
}
