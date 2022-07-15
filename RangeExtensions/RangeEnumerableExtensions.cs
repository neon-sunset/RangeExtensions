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

        var array = new int[length];
        var enumerator = enumerable.GetEnumerator();

        for (var i = 0; i < array.Length; i++)
        {
            enumerator.MoveNext();
            array[i] = enumerator.Current;
        }

        return array;
    }
}
