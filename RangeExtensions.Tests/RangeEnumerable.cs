namespace RangeExtensions.Tests;

public class RangeEnumerableTests
{
    private static IEnumerable<object[]> InvalidRanges()
    {
        yield return new object[] { 0..^10 };
        yield return new object[] { ^5..10 };
        yield return new object[] { ^5..^10 };
        yield return new object[] { ^5..^10 };
        yield return new object[] { 0.. };
        yield return new object[] { 10.. };
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(177013)]
    public void AscendingRangeOutput_MatchesEnumerableRange(int count)
    {
        var enumerable = Enumerable.Range(0, count);
        var range = (0..count).AsEnumerable();

        Assert.Equal(enumerable, range);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(177013)]
    public void DescendingRangeOutput_MatchesEnumerableRange(int count)
    {
        var enumerable = Enumerable.Range(0, count).Reverse();
        var range = (count..0).AsEnumerable();

        Assert.Equal(enumerable, range);
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
}
