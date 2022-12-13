namespace System.Linq;

public readonly partial record struct RangeEnumerable : ICollection<int>
{
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _start < _end
                ? _end - _start
                : _start - _end;
        }
    }

    public bool IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => true;
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    public void Add(int item)
    {
        throw new NotSupportedException();
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    public void Clear()
    {
        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(int item)
    {
        if (Count is 0)
        {
            return false;
        }

        var (min, max) = MinAndMaxUnchecked();

        return item >= min && item <= max;
    }

    public void CopyTo(int[] array, int index)
    {
        if (index < 0 || index > array.Length)
        {
            ThrowHelpers.ArgumentOutOfRange();
        }

        var count = Count;
        if (count > (array.Length - index))
        {
            ThrowHelpers.ArgumentException();
        }

        var enumerator = GetEnumerator();

        for (var i = index; i < count; i++)
        {
            _ = enumerator.MoveNext();

            array[i] = enumerator.Current;
        }
    }

#if !NETSTANDARD2_0
    public void CopyTo(Span<int> span, int index)
    {
        if (index < 0 || index > span.Length)
        {
            ThrowHelpers.ArgumentOutOfRange();
        }

        var count = Count;
        if (count > (span.Length - index))
        {
            ThrowHelpers.ArgumentException();
        }

        var enumerator = GetEnumerator();

        // TODO: SIMDify because why not
        // TODO: Verify that bounds check is elided in codegen
        for (var i = index; i < count; i++)
        {
            _ = enumerator.MoveNext();

            span[i] = enumerator.Current;
        }
    }
#endif

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    public bool Remove(int item)
    {
        throw new NotSupportedException();
    }
}
