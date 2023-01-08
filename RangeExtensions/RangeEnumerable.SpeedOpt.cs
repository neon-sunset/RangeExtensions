namespace System.Linq;

public readonly partial record struct RangeEnumerable
{
    public int Aggregate(Func<int, int, int> func)
    {
        ThrowHelpers.CheckNull(func);
        ThrowHelpers.CheckEmpty(this);

        var enumerator = GetEnumeratorUnchecked();
        enumerator.MoveNext();

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any()
    {
        return Count > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Average()
    {
        ThrowHelpers.CheckEmpty(this);

        var (first, last) = FirstAndLastUnchecked();

        return ((double)first + last) / 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable Distinct()
    {
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int First()
    {
        ThrowHelpers.CheckEmpty(this);

        return _start < _end
            ? _start
            : _start - 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Last()
    {
        ThrowHelpers.CheckEmpty(this);

        return _start < _end
            ? _end - 1
            : _end;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Max()
    {
        ThrowHelpers.CheckEmpty(this);

        var (first, last) = FirstAndLastUnchecked();

        return Math.Max(first, last);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Min()
    {
        ThrowHelpers.CheckEmpty(this);

        var (first, last) = FirstAndLastUnchecked();

        return Math.Min(first, last);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable Reverse()
    {
        return new(start: _end, end: _start);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SelectRange<T> Select<T>(Func<int, T> selector)
    {
        return new(selector, _start, _end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable Skip(int count)
    {
        if (count >= Count)
        {
            return Empty;
        }

        if (count <= 0)
        {
            return this;
        }

        var start = IsAscending()
            ? _start + count
            : _start - count;

        return new(start, _end);
    }

    public int Sum()
    {
        var (min, max) = MinAndMaxUnchecked();
        if (min <= 1)
        {
            var longMax = (long)max;
            var longSum = longMax * (longMax + 1) / 2;

            return checked((int)longSum);
        }

        var sum = 0;
        for (var i = min; i <= max; i++)
        {
            sum = checked(sum + i);
        }

        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeEnumerable Take(int count)
    {
        if (count >= Count)
        {
            return this;
        }

        if (count <= 0)
        {
            return Empty;
        }

        var end = IsAscending()
            ? _start + count
            : _start - count;

        return new(_start, end);
    }

    public int[] ToArray()
    {
        var length = Count;
        if (length is 0)
        {
            return Array.Empty<int>();
        }

#if NET6_0_OR_GREATER
        var array = GC.AllocateUninitializedArray<int>(length);
#else
        var array = new int[length];
#endif

#if NETCOREAPP3_1 || NET
        InitializeSpan(_start, _end, array);
        return array;
#else
        var enumerator = GetEnumeratorUnchecked();
        for (var i = 0; i < array.Length; i++)
        {
            enumerator.MoveNext();
            array[i] = enumerator.Current;
        }

        return array;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WhereRange Where(Func<int, bool> predicate)
    {
        return new(predicate, _start, _end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Deconstruct(out int start, out int end)
    {
        start = _start;
        end = _end;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal (int, int) MinAndMaxUnchecked()
    {
        return _start <= _end
            ? (_start, _end - 1)
            : (_end, _start - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (int, int) FirstAndLastUnchecked()
    {
        return _start <= _end
            ? (_start, _end - 1)
            : (_start - 1, _end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsAscending()
    {
        return _start <= _end;
    }
}
