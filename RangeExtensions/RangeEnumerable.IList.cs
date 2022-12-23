namespace System.Linq;

public readonly partial record struct RangeEnumerable : IList<int>
{
    public int this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= Count || _start == _end)
            {
                IndexOutOfRange();
            }

            return _start < _end
                ? _start + index
                : _start - index - 1;
        }
        set => throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(int item)
    {
        var count = Count;
        var index = _start < _end
            ? item - _start
            : _start - 1 - item;

        return index >= 0 && index < count
            ? index : -1;
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
        throw new ArgumentOutOfRangeException(
            "Index was outside the bounds of the enumerable range.");
    }
}
