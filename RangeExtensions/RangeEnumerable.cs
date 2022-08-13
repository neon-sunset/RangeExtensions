using System.Collections;
using System.Diagnostics;

namespace System.Linq;

public readonly partial record struct RangeEnumerable
{
    public static RangeEnumerable Empty => default;

    internal readonly Range Range;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable(Range range)
    {
        ThrowHelpers.CheckInvalid(range);

        Range = range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal RangeEnumerable(Range range, bool skipValidation)
    {
        Debug.Assert(skipValidation);

        Range = range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator()
    {
        return GetEnumeratorUnchecked();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Enumerator GetEnumeratorUnchecked()
    {
        return new Enumerator(Range, skipValidation: true);
    }

    public record struct Enumerator : IEnumerator<int>
    {
        private readonly int _shift;
        private readonly int _end;

        private int _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator(Range range)
        {
            ThrowHelpers.CheckInvalid(range);

            var start = range.Start.Value;
            var end = range.End.Value;

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
        internal Enumerator(Range range, bool skipValidation)
        {
            Debug.Assert(skipValidation);

            var start = range.Start.Value;
            var end = range.End.Value;

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

    public static implicit operator RangeEnumerable(Range range) => new(range);

    public static implicit operator Range(RangeEnumerable enumerable) => enumerable.Range;
}
