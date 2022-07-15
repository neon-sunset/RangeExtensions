namespace System;

public static class RangeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerator(range);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] ToArray(this Range range)
    {
        return range.AsEnumerable().ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<int> ToList(this Range range)
    {
        return range.AsEnumerable().ToList();
    }
}
