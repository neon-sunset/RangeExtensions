namespace RangeExtensions;

internal static class ThrowHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckInvalid(int start, int end)
    {
        if (start < 0 || end < 0)
        {
            InvalidRange(start, end);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckEmpty(RangeEnumerable enumerable)
    {
        var (start, end) = enumerable;
        if (start == end)
        {
            EmptyRange();
        }
    }

    // TODO: Replace with generic constrant T where T : IRangeSource or similar?
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckEmpty(int start, int end)
    {
        if (start == end)
        {
            EmptyRange();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckNull<T>(T value) where T : class
    {
        if (value is null)
        {
            ArgumentNull();
        }
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    public static void ArgumentException()
    {
        throw new ArgumentException();
    }

    #if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    public static void ArgumentNull()
    {
        throw new ArgumentNullException();
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
    private static void InvalidRange(int start, int end)
    {
        throw new ArgumentOutOfRangeException(nameof(Range), start..end, "Cannot enumerate numbers in range with a head or tail indexed from end.");
    }

#if NETSTANDARD2_0
    [MethodImpl(MethodImplOptions.NoInlining)]
#else
    [DoesNotReturn]
#endif
    private static void EmptyRange()
    {
        throw new InvalidOperationException("Sequence contains no elements");
    }
}
