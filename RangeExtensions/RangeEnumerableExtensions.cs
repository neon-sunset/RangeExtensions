using System.Diagnostics.CodeAnalysis;

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
        return range.Count() > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Average(this RangeEnumerable enumerable)
    {
        return ((double)enumerable.GetFirst() + enumerable.GetLast()) / 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(this RangeEnumerable enumerable, int value)
    {
        if (enumerable.Count() is 0)
        {
            return false;
        }

        return value >= enumerable.Min() && value <= enumerable.Max();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable Distinct(this RangeEnumerable enumerable)
    {
        return enumerable;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int First(this RangeEnumerable enumerable)
    {
        return enumerable.GetFirst();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Last(this RangeEnumerable enumerable)
    {
        return enumerable.GetLast();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(this RangeEnumerable enumerable)
    {
        return Math.Max(enumerable.GetFirst(), enumerable.GetLast());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(this RangeEnumerable enumerable)
    {
        return Math.Min(enumerable.GetFirst(), enumerable.GetLast());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable Reverse(this RangeEnumerable enumerable)
    {
        var reversed = new Range(enumerable.Range.End, enumerable.Range.Start);

        return new RangeEnumerable(reversed, skipValidation: true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable Skip(this RangeEnumerable enumerable, int count)
    {
        if (count >= enumerable.Count())
        {
            return RangeEnumerable.Empty;
        }

        if (count <= 0)
        {
            return enumerable;
        }

        var initialRange = enumerable.Range;
        var newStart = enumerable.Direction is RangeDirection.Ascending
            ? initialRange.Start.Value + count
            : initialRange.Start.Value - count;

        return new RangeEnumerable(new Range(newStart, initialRange.End), skipValidation: true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable Take(this RangeEnumerable enumerable, int count)
    {
        if (count >= enumerable.Count())
        {
            return enumerable;
        }

        if (count <= 0)
        {
            return RangeEnumerable.Empty;
        }

        var initialRange = enumerable.Range;
        var newEnd = enumerable.Direction is RangeDirection.Ascending
            ? initialRange.Start.Value + count
            : initialRange.Start.Value - count;

        return new RangeEnumerable(new Range(initialRange.Start, newEnd), skipValidation: true);
    }

    public static int[] ToArray(this RangeEnumerable enumerable)
    {
        var length = enumerable.Count();
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
        var length = enumerable.Count();
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
