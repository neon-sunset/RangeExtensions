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
        var range = (0..count).AsEnumerable();
        var enumerable = Enumerable.Range(0, count);

        Assert.Equal(enumerable, range);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(177013)]
    public void AscendingRangeEnumeratorOutput_MatchesEnumerableRange(int count)
    {
        var range = 0..count;
        var enumerable = Enumerable.Range(0, count);
        
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
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(177013)]
    public void DescendingRangeOutput_MatchesEnumerableRange(int count)
    {
        var range = (count..0).AsEnumerable();
        var enumerable = Enumerable.Range(0, count).Reverse();

        Assert.Equal(enumerable, range);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(177013)]
    public void DescendingRangeEnumeratorOutput_MatchesEnumerableRange(int count)
    {
        var range = count..0;
        var enumerable = Enumerable.Range(0, count).Reverse();

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
