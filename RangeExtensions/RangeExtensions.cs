using System.Runtime.CompilerServices;
using RangeExtensions;

namespace System;

public static class RangeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerable AsEnumerable(this Range range)
    {
        return new RangeEnumerable(range);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerator(range);
    }
}
