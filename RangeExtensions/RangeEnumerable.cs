using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

internal enum RangeDirection { Ascending, Descending }

public readonly record struct RangeEnumerable : IEnumerable<int>
{
    internal static readonly RangeEnumerable Empty = new();

    internal readonly Range Range;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable(Range range)
    {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
        {
            InvalidRange(range);
        }

        Range = range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal RangeEnumerable(Range range, bool skipValidation)
    {
        Debug.Assert(skipValidation);

        Range = range;
    }

    internal RangeDirection Direction => Range.Start.Value < Range.End.Value
        ? RangeDirection.Ascending
        : RangeDirection.Descending;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal int GetFirst()
    {
        if (Range.Start.Value == Range.End.Value)
        {
            EmptyRange();
        }

        return Range.Start.Value < Range.End.Value
        ? Range.Start.Value
        : Range.Start.Value - 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal int GetLast()
    {
        if (Range.Start.Value == Range.End.Value)
        {
            EmptyRange();
        }

        return Range.End.Value > Range.Start.Value
        ? Range.End.Value - 1
        : Range.End.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count()
    {
        var start = Range.Start.Value;
        var end = Range.End.Value;

        return start < end
            ? end - start
            : start - end;
    }

    public RangeEnumerator GetEnumerator() => Range.GetEnumeratorUnchecked();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => Range.GetEnumeratorUnchecked();
    IEnumerator IEnumerable.GetEnumerator() => Range.GetEnumeratorUnchecked();

    public static implicit operator RangeEnumerable(Range range) => new(range);

    public static implicit operator Range(RangeEnumerable enumerable) => enumerable.Range;

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    private static void InvalidRange(Range range)
    {
        throw new ArgumentOutOfRangeException(nameof(range), range, "Cannot enumerate numbers in range with a head or tail indexed from end.");
    }

    private static void EmptyRange()
    {
        throw new InvalidOperationException("Range constains no elements.");
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
    internal RangeEnumerator(Range range, bool skipValidation)
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
