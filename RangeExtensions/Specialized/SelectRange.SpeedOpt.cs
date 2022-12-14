namespace RangeExtensions;

public readonly partial record struct SelectRange<T>
{
    public T Aggregate(Func<T, T, T> func)
    {
        ThrowHelpers.CheckNull(func);
        ThrowHelpers.CheckEmpty(_start, _end);

        var enumerator = GetEnumerator();
        enumerator.MoveNext();

        var result = enumerator.Current;
        while (enumerator.MoveNext())
        {
            result = func(result, enumerator.Current);
        }

        return result;
    }

    public TAccumulate Aggregate<TAccumulate>(
        TAccumulate seed, Func<TAccumulate, T, TAccumulate> func)
    {
        ThrowHelpers.CheckNull(func);

        var result = seed;
        foreach (var element in this)
        {
            result = func(result, element);
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any()
    {
        return Count > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T First()
    {
        ThrowHelpers.CheckEmpty(_start, _end);

        var num = _start < _end
            ? _start
            : _start - 1;

        return _selector(num);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? FirstOrDefault()
    {
        return FirstOrDefault(default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? FirstOrDefault(T? defaultValue)
    {
        if (_start == _end)
        {
            return defaultValue;
        }

        var num = _start < _end
            ? _start
            : _start - 1;

        return _selector(num);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Last()
    {
        ThrowHelpers.CheckEmpty(_start, _end);

        var num = _start < _end
            ? _end - 1
            : _end;

        return _selector(num);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? LastOrDefault()
    {
        return LastOrDefault(default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? LastOrDefault(T? defaultValue)
    {
        if (_start == _end)
        {
            return defaultValue;
        }

        var num = _start < _end
            ? _end - 1
            : _end;

        return _selector(num);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SelectRange<T> Reverse()
    {
        return new(_selector, start: _end, end: _start);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SelectRange<TResult> Select<TResult>(Func<T, TResult> selector)
    {
        ThrowHelpers.CheckNull(selector);

        var inner = _selector;
        TResult Outer(int i) => selector(inner(i));

        return new(Outer, _start, _end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SelectRange<T> Skip(int count)
    {
        if (count >= Count)
        {
            return new(_selector, 0, 0);
        }

        if (count <= 0)
        {
            return this;
        }

        var start = _start <= _end
            ? _start + count
            : _start - count;

        return new(_selector, start, _end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SelectRange<T> Take(int count)
    {
        if (count >= Count)
        {
            return this;
        }

        if (count <= 0)
        {
            return new(_selector, 0, 0);
        }

        var end = _start <= _end
            ? _start + count
            : _start - count;

        return new(_selector, _start, end);
    }
}
