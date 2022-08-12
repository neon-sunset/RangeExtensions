using System.Collections;

namespace RangeExtensions.Tests;

public partial class RangeEnumerableTests
{
    [Theory, MemberData(nameof(ValidRangePairs))]
    public void RangeEnumerator_MatchesStandardEnumerableRange(Range range, IEnumerable<int> enumerable)
    {
        IEnumerable<int> Enumerate()
        {
            foreach (var i in range)
            {
                yield return i;
            }
        }

        Assert.Equal(enumerable, Enumerate());
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void RangeEnumerableBoxed_MatchesStandardEnumerableRange(Range range, IEnumerable<int> enumerable)
    {
        IEnumerable<int> EnumerateBoxed()
        {
            foreach (var i in (IEnumerable)range.AsEnumerable())
            {
                yield return (int)i!;
            }
        }

        Assert.Equal(enumerable, EnumerateBoxed());
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void RangeEnumerableBoxedGeneric_MatchesStandardEnumerableRange(Range range, IEnumerable<int> enumerable)
    {
        IEnumerable<int> EnumerateBoxed()
        {
            foreach (var i in (IEnumerable<int>)range.AsEnumerable())
            {
                yield return i;
            }
        }

        Assert.Equal(enumerable, EnumerateBoxed());
    }
}
