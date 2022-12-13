namespace RangeExtensions;

public readonly partial record struct WhereRange
{
    public int First()
    {
        ThrowHelpers.CheckEmpty(_start, _end);

        foreach (var num in new RangeEnumerable(
            start: _start,
            end: _end))
        {
            if (_predicate(num))
            {
                return num;
            }
        }

        return ThrowEmpty();
    }

    public int FirstOrDefault()
    {
        return FirstOrDefault(default);
    }

    public int FirstOrDefault(int defaultValue)
    {
        if (_start == _end)
        {
            return defaultValue;
        }

        foreach (var num in new RangeEnumerable(
            start: _start,
            end: _end))
        {
            if (_predicate(num))
            {
                return num;
            }
        }

        return defaultValue;
    }

    public int Last()
    {
        ThrowHelpers.CheckEmpty(_start, _end);

        foreach (var num in new RangeEnumerable(
            start: _end,
            end: _start))
        {
            if (_predicate(num))
            {
                return num;
            }
        }

        return ThrowEmpty();
    }

    public int LastOrDefault()
    {
        return LastOrDefault(default);
    }

    public int LastOrDefault(int defaultValue)
    {
        if (_start == _end)
        {
            return defaultValue;
        }

        foreach (var num in new RangeEnumerable(
            start: _end,
            end: _start))
        {
            if (_predicate(num))
            {
                return num;
            }
        }

        return defaultValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WhereRange Reverse()
    {
        return new(_predicate, start: _end, end: _start);
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    private static int ThrowEmpty()
    {
        throw new InvalidOperationException("Sequence contains no elements");
    }
}
