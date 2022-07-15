using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RangeExtensions;

public readonly struct RangeEnumerable : IEnumerable<int>
{
    private readonly Range _range;

    public RangeEnumerable(Range range)
    {
        _range = range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<int> GetEnumerator() => _range.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _range.GetEnumerator();

    public static implicit operator RangeEnumerable(Range range) => new(range);

    public static implicit operator Range(RangeEnumerable enumerable) => enumerable._range;
}

public struct RangeEnumerator : IEnumerator<int>
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

    [DoesNotReturn]
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
