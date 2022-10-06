namespace System.Linq;

public static class RangeEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable AsEnumerable(this Range range)
    {
        return new RangeEnumerable(range);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any(this RangeEnumerable range)
    {
        return range.Count > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Average(this RangeEnumerable enumerable)
    {
        ThrowHelpers.CheckEmpty(enumerable);

        var (first, last) = FirstAndLastUnchecked(enumerable);

        return ((double)first + last) / 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count(this RangeEnumerable enumerable)
    {
        return enumerable.Count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable Distinct(this RangeEnumerable enumerable)
    {
        return enumerable;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int First(this RangeEnumerable enumerable)
    {
        ThrowHelpers.CheckEmpty(enumerable);

        var start = enumerable.Range.Start.Value;
        var end = enumerable.Range.End.Value;

        return start < end ? start : start - 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Last(this RangeEnumerable enumerable)
    {
        ThrowHelpers.CheckEmpty(enumerable);

        var start = enumerable.Range.Start.Value;
        var end = enumerable.Range.End.Value;

        return start < end ? end - 1 : end;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(this RangeEnumerable enumerable)
    {
        ThrowHelpers.CheckEmpty(enumerable);

        var (first, last) = FirstAndLastUnchecked(enumerable);

        return Math.Max(first, last);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(this RangeEnumerable enumerable)
    {
        ThrowHelpers.CheckEmpty(enumerable);

        var (first, last) = FirstAndLastUnchecked(enumerable);

        return Math.Min(first, last);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable Reverse(this RangeEnumerable enumerable)
    {
        var reversed = new Range(enumerable.Range.End, enumerable.Range.Start);

        return new RangeEnumerable(reversed, skipValidation: true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SelectEnumerable<T> Select<T>(this RangeEnumerable enumerable, Func<int, T> selector)
    {
        return new SelectEnumerable<T>(selector, enumerable);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable Skip(this RangeEnumerable enumerable, int count)
    {
        if (count >= enumerable.Count)
        {
            return RangeEnumerable.Empty;
        }

        if (count <= 0)
        {
            return enumerable;
        }

        var initialRange = enumerable.Range;
        var newStart = IsAscending(enumerable)
            ? initialRange.Start.Value + count
            : initialRange.Start.Value - count;

        return new RangeEnumerable(new Range(newStart, initialRange.End), skipValidation: true);
    }

    public static int Sum(this RangeEnumerable enumerable)
    {
        var (min, max) = MinAndMaxUnchecked(enumerable);

        if (min <= 1)
        {
            var longMax = (long)max;
            var longSum = longMax * (longMax + 1) / 2;

            return checked((int)longSum);
        }

        var sum = 0;
        for (var i = min; i <= max; i++)
        {
            sum = checked(sum + i);
        }

        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable Take(this RangeEnumerable enumerable, int count)
    {
        if (count >= enumerable.Count)
        {
            return enumerable;
        }

        if (count <= 0)
        {
            return RangeEnumerable.Empty;
        }

        var initialRange = enumerable.Range;
        var newEnd = IsAscending(enumerable)
            ? initialRange.Start.Value + count
            : initialRange.Start.Value - count;

        return new RangeEnumerable(new Range(initialRange.Start, newEnd), skipValidation: true);
    }

    public static int[] ToArray(this RangeEnumerable enumerable)
    {
        var length = enumerable.Count;
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
        var length = enumerable.Count;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static (int, int) MinAndMaxUnchecked(this RangeEnumerable enumerable)
    {
        var (start, end) = enumerable.Range.GetStartAndEnd();

        return start <= end
            ? (start, end - 1)
            : (end, start - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (int, int) FirstAndLastUnchecked(this RangeEnumerable enumerable)
    {
        var (start, end) = enumerable.Range.GetStartAndEnd();

        return start <= end
            ? (start, end - 1)
            : (start - 1, end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsAscending(this RangeEnumerable enumerable)
    {
        return enumerable.Range.Start.Value <= enumerable.Range.End.Value;
    }
}
