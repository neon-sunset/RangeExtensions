using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace RangeExtensions;

public readonly record struct RangeEnumerable : IEnumerable<int>
{
    private readonly Range _range;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable(Range range)
    {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
        {
            InvalidRange(range);
        }

        _range = range;
    }

    public int Length
    {
        get
        {
            var start = _range.Start.Value;
            var end = _range.End.Value;

            return start < end
                ? end - start
                : start - end;
        }
    }

    public RangeEnumerator GetEnumerator() => _range.GetEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => _range.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _range.GetEnumerator();

    public static implicit operator RangeEnumerable(Range range) => new(range);

    public static implicit operator Range(RangeEnumerable enumerable) => enumerable._range;

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    private static void InvalidRange(Range range)
    {
        throw new ArgumentOutOfRangeException(nameof(range), range, "Cannot enumerate numbers in range with a head or tail indexed from end.");
    }
}

public record struct RangeEnumerator : IEnumerator<int>
{
    private readonly int _shift;
    private readonly int _end;
    private int _current;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerator(Range range)
    {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
        {
            InvalidRange(range);
        }

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

    public int Current => _current;
    object IEnumerator.Current => _current;

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    private static void InvalidRange(Range range)
    {
        throw new ArgumentOutOfRangeException(nameof(range), range, "Cannot enumerate numbers in range with a head or tail indexed from end.");
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }

    public void Dispose() { }
}
