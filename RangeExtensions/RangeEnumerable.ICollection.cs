namespace System.Linq;

public readonly partial record struct RangeEnumerable : ICollection<int>
{
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var start = Range.Start.Value;
            var end = Range.End.Value;

            return start < end
                ? end - start
                : start - end;
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

        var (min, max) = this.MinAndMaxUnchecked();

        return item >= min && item <= max;
    }

    public void CopyTo(int[] array, int arrayIndex)
    {
        var arrayLength = array.Length;

        if (arrayIndex is 0 || arrayIndex > arrayLength)
        {
            ThrowHelpers.ArgumentOutOfRange();
        }

        var rangeCount = Count;
        if (rangeCount > (arrayLength - arrayIndex))
        {
            ThrowHelpers.ArgumentOutOfRange();
        }

        var enumerator = GetEnumerator();

        for (var i = arrayIndex; i < arrayLength; i++)
        {
            _ = enumerator.MoveNext();

            array[i] = enumerator.Current;
        }
    }

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
