namespace System;

public static class RangeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable AsEnumerable(this Range range)
    {
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable.Enumerator GetEnumerator(this Range range)
    {
        return range.AsEnumerable().GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Aggregate(this Range range, Func<int, int, int> func)
    {
        return range.AsEnumerable().Aggregate(func);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TAccumulate Aggregate<TAccumulate>(
        this Range range, TAccumulate seed, Func<TAccumulate, int, TAccumulate> func)
    {
        return range.AsEnumerable().Aggregate(seed, func);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectRange<T> Select<T>(this Range range, Func<int, T> selector)
    {
        return range.AsEnumerable().Select(selector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WhereRange Where(this Range range, Func<int, bool> predicate)
    {
        return range.AsEnumerable().Where(predicate);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] ToArray(this Range range)
    {
        return range.AsEnumerable().ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<int> ToList(this Range range)
    {
        return new(range.AsEnumerable());
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
    internal static (int, int) UnwrapUnchecked(this Range range)
    {
        var (start, end) = (range.Start, range.End);

#if NETCOREAPP3_1 || NET6_0_OR_GREATER
        return (
            Unsafe.As<Index, int>(ref start),
            Unsafe.As<Index, int>(ref end));
#else
        return (start.Value, end.Value);
#endif
    }
}
