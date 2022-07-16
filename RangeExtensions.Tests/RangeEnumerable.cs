namespace RangeExtensions.Tests;

public class RangeEnumerableTests
{
    private static IEnumerable<object[]> ValidRangePairs() => Data.ValidRangePairs();
    private static IEnumerable<object[]> InvalidRanges() => Data.InvalidRanges();

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void RangeEnumerable_MatchesStandardEnumerableRange(Range range, IEnumerable<int> enumerable)
    {
        Assert.Equal(enumerable, range.AsEnumerable());
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void AscendingRangeEnumeratorOutput_MatchesEnumerableRange(Range range, IEnumerable<int> enumerable)
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

    [Theory]
    [MemberData(nameof(InvalidRanges))]
    public void RangeEnumerable_ThrowsOnInvalidRange(Range range)
    {
        void AsEnumerable()
        {
            _ = range.AsEnumerable();
        }

        Assert.Throws<ArgumentOutOfRangeException>(AsEnumerable);
    }

    [Theory]
    [MemberData(nameof(InvalidRanges))]
    public void RangeEnumerator_ThrowsOnInvalidRange(Range range)
    {
        void Enumerate()
        {
            foreach (var i in range) { }
        }

        Assert.Throws<ArgumentOutOfRangeException>(Enumerate);
    }
}
