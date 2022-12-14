namespace RangeExtensions;

public readonly partial record struct WhereRange
{
    public int Aggregate(Func<int, int, int> func)
    {
        ThrowHelpers.CheckNull(func);
        ThrowHelpers.CheckEmpty(_start, _end);

        var enumerator = GetEnumerator();
        if (!enumerator.MoveNext())
        {
            ThrowHelpers.EmptyRange();
        }

        var result = enumerator.Current;
        while (enumerator.MoveNext())
        {
            result = func(result, enumerator.Current);
        }

        return result;
    }

    public TAccumulate Aggregate<TAccumulate>(
        TAccumulate seed, Func<TAccumulate, int, TAccumulate> func)
    {
        ThrowHelpers.CheckNull(func);

        var result = seed;
        foreach (var element in this)
        {
            result = func(result, element);
        }

        return result;
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WhereRange Where(Func<int, bool> predicate)
    {
        ThrowHelpers.CheckNull(predicate);

        var inner = _predicate;
        bool Outer(int i) => inner(i) && predicate(i);

        return new(Outer, _start, _end);
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
