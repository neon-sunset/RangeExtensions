namespace System;

public static class RangeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable.Enumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerable.Enumerator(range);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static (int, int) GetStartAndEnd(this Range range)
    {
        return (range.Start.Value, range.End.Value);
    }
}
