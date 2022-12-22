namespace System.Linq;

public readonly partial record struct RangeEnumerable : IList<int>
{
    public int this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ThrowHelpers.CheckEmpty(this);

            if (_start < _end)
            {
                var result = _start + index;
                if (result >= _end)
                {
                    IndexOutOfRange();
                }

                return result;
            }
            else
            {
                var result = _start - index - 1;
                if (result < _end)
                {
                    IndexOutOfRange();
                }

                return result;
            }
        }
        set => throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(int item)
    {
        if (_start == _end)
        {
            return -1;
        }

        var index = _start < _end
            ? item - _start
            : _start - 1 - item;

        return Math.Max(index, -1);
    }

    public void Insert(int index, int item)
    {
        throw new NotSupportedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotSupportedException();
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    private static void IndexOutOfRange()
    {
        throw new IndexOutOfRangeException(
            "Index was outside the bounds of the enumerable range.");
    }
}
