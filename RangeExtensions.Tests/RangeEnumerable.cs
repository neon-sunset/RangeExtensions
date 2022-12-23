namespace RangeExtensions.Tests;

public partial class RangeEnumerableTests
{
    public static IEnumerable<object[]> ValidRangePairs() => Data.ValidRangePairs();
    public static IEnumerable<object[]> InvalidRanges() => Data.InvalidRanges();

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void RangeEnumerable_MatchesStandardEnumerableRange(Range range, IEnumerable<int> enumerable)
    {
        Assert.Equal(enumerable, range.AsEnumerable());
    }

    [Theory, MemberData(nameof(InvalidRanges))]
    public void RangeEnumerable_ThrowsOnInvalidRange(Range range)
    {
        void AsEnumerable()
        {
            _ = range.AsEnumerable();
        }

        Assert.Throws<ArgumentOutOfRangeException>(AsEnumerable);
    }

    [Theory, MemberData(nameof(InvalidRanges))]
    public void RangeEnumerator_ThrowsOnInvalidRange(Range range)
    {
        void Enumerate()
        {
            foreach (var i in range) { }
        }

        Assert.Throws<ArgumentOutOfRangeException>(Enumerate);
    }

    [Fact]
    public void RangeEnumerator_ResetThrows()
    {
        static void Reset()
        {
            (0..int.MaxValue).GetEnumerator().Reset();
        }

        Assert.Throws<NotSupportedException>(Reset);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void RangeEnumerable_ImplicitConversionToRange_KeepsRangeIntact(Range range, IEnumerable<int> enumerable)
    {
        _ = enumerable;
        var rangeEnumerable = range.AsEnumerable();

        Assert.Equal<Range>(range, rangeEnumerable);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void RangeEnumerable_ImplicitConversionFromRange_KeepsRangeIntact(Range range, IEnumerable<int> enumerable)
    {
        _ = enumerable;
        var rangeEnumerable = range.AsEnumerable();

        Assert.Equal<RangeEnumerable>(rangeEnumerable, range);
    }

    [Theory, MemberData(nameof(InvalidRanges))]
    public void RangeEnumerable_ImplicitConversionFromRange_ThrowsOnInvalidRange(Range range)
    {
        void ToRangeEnumerable()
        {
            RangeEnumerable _ = range;
        }

        Assert.Throws<ArgumentOutOfRangeException>(ToRangeEnumerable);
    }
}
