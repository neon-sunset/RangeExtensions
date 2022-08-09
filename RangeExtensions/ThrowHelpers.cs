namespace RangeExtensions;

internal static class ThrowHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckInvalid(Range range)
    {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
        {
            InvalidRange(range);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckEmpty(RangeEnumerable enumerable)
    {
        var start = enumerable.Range.Start.Value;
        var end = enumerable.Range.End.Value;

        if (start == end)
        {
            EmptyRange();
        }
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    public static void ArgumentOutOfRange()
    {
        throw new ArgumentOutOfRangeException();
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    private static void InvalidRange(Range range)
    {
        throw new ArgumentOutOfRangeException(nameof(range), range, "Cannot enumerate numbers in range with a head or tail indexed from end.");
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    private static void EmptyRange()
    {
        throw new InvalidOperationException("Range constains no elements.");
    }
}
