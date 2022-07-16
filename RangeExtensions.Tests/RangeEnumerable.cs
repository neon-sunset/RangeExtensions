namespace RangeExtensions.Tests;

public class RangeEnumerableTests
{
    [Fact]
    public void AscendingRangeOutput_MatchesEnumerableRange()
    {
        var enumerable = Enumerable.Range(0, 10);
        var range = (0..10).AsEnumerable();

        Assert.Equal(enumerable, range);
    }

    [Fact]
    public void DescendingRangeOutput_MatchesEnumerableRange()
    {
        var enumerable = Enumerable.Range(0, 10).Reverse();
        var range = (10..0).AsEnumerable();

        Assert.Equal(enumerable, range);
    }
}
