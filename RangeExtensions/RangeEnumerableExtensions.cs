namespace System.Linq;

public static class RangeEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable AsEnumerable(this Range range)
    {
        return new RangeEnumerable(range);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count(this RangeEnumerable enumerable)
    {
        return enumerable.Length;
    }

    public static int[] ToArray(this RangeEnumerable enumerable)
    {
        var length = enumerable.Length;
        if (length is 0)
        {
            return Array.Empty<int>();
        }

#if NET6_0_OR_GREATER
        var array = GC.AllocateUninitializedArray<int>(length);
#else
        var array = new int[length];
#endif
        var enumerator = enumerable.GetEnumerator();

        for (var i = 0; i < array.Length; i++)
        {
            enumerator.MoveNext();
            array[i] = enumerator.Current;
        }

        return array;
    }

    public static List<int> ToList(this RangeEnumerable enumerable)
    {
        var length = enumerable.Length;
        if (length is 0)
        {
            return new List<int>(0);
        }

        var list = new List<int>(length);
        foreach (var i in enumerable)
        {
            list.Add(i);
        }

        return list;
    }
}
