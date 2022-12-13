using System.Collections;

namespace System.Linq;

public readonly partial record struct RangeEnumerable
{
    private readonly int _start;
    private readonly int _end;

    public static RangeEnumerable Empty => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable(Range range)
    {
        var (start, end) = range.UnwrapUnchecked();

// Sadly we need this to both avoid codegen regressions pre-net8.0 and comply with tests on netstandard
#if NETCOREAPP3_1 || NET6_0_OR_GREATER
        ThrowHelpers.CheckInvalid(start, end);
#else
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
        {
            ThrowHelpers.InvalidRange(start, end);
        }
#endif

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
        return new(_start, _end);
    }

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
