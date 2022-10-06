namespace System;

public static class RangeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable.Enumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerable.Enumerator(range);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectEnumerable<T> Select<T>(this Range range, Func<int, T> selector)
    {
        return range.AsEnumerable().Select(selector);
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

#if !NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo(this Range range, Span<int> destination)
    {
        range.AsEnumerable().CopyTo(destination, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCopyTo(this Range range, Span<int> destination)
    {
        var result = false;
        var enumerable = range.AsEnumerable();

        if (enumerable.Count <= destination.Length)
        {
            enumerable.CopyTo(destination, 0);
            result = true;
        }
        
        return result;
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static (int, int) GetStartAndEnd(this Range range)
    {
        return (range.Start.Value, range.End.Value);
    }
}
