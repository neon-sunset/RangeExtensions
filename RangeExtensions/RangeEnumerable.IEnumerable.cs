using System.Collections;

namespace System.Linq;

public readonly partial record struct RangeEnumerable : IEnumerable<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    {
        return GetEnumeratorUnchecked();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumeratorUnchecked();
    }
}
