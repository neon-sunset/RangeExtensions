namespace RangeExtensions.Tests;

public partial class WhereRangeTests
{
    public static IEnumerable<object[]> EmptyRanges() => Data.EmptyRanges();

    [Theory, MemberData(nameof(ValidRangePairs))]
    public static void Aggregate_MatchesIEnumerableAggregate(Range range, IEnumerable<int> enumerable)
    {
        AssertHelpers.EqualValueOrException(
            () => enumerable.Where(i => i % 2 != 0).Aggregate((acc, i) => acc + i),
            () => range.Where(i => i % 2 != 0).Aggregate((acc, i) => acc + i));
    }

    [Fact]
    public static void Aggregate_ThrowsOnNullDelegate()
    {
        static void AggregateNull()
        {
            _ = (0..100).Where(i => i % 2 != 0).Aggregate(null!);
        }

        Assert.Throws<ArgumentNullException>(AggregateNull);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public static void AggregateWithSeed_MatchesIEnumerableAggregateWithSeed(Range range, IEnumerable<int> enumerable)
    {
        const int seed = 34;

        var expected = enumerable.Where(i => i % 2 != 0).Aggregate(seed, (acc, i) => acc + i);
        var actual = range.Where(i => i % 2 != 0).Aggregate(seed, (acc, i) => acc + i);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void AggregateWithSeed_ThrowsOnNullDelegate()
    {
        static void AggregateNull()
        {
            _ = (0..100).Where(i => i % 2 != 0).Aggregate(34, (Func<long, int, long>)null!);
        }

        Assert.Throws<ArgumentNullException>(AggregateNull);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void First_MatchesIEnumerableFirst(Range range, IEnumerable<int> enumerable)
    {
        var whereRange = range.Where(i => i % 2 != 0);
        if (whereRange.Count() is 0)
        {
            return;
        }

        var expected = enumerable.First(i => i % 2 != 0);
        var actual = whereRange.First();

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(EmptyRanges))]
    public void First_ThrowsOnEmptyRange(Range range)
    {
        void First()
        {
            _ = range.Where(i => i % 2 != 0).First();
        }

        Assert.Throws<InvalidOperationException>(First);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void FirstOrDefault_MatchesIEnumerableFirstOrDefault(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.Where(i => i % 2 != 0);
        var actual = range.Where(i => i % 2 != 0);

        Assert.Equal(expected.FirstOrDefault(), actual.FirstOrDefault());
#if NET6_0_OR_GREATER
        Assert.Equal(expected.FirstOrDefault(42), actual.FirstOrDefault(42));
#endif
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Last_MatchesIEnumerableLast(Range range, IEnumerable<int> enumerable)
    {
        var whereRange = range.Where(i => i % 2 != 0);
        if (whereRange.Count() is 0)
        {
            return;
        }

        var expected = enumerable.Last(i => i % 2 != 0);
        var actual = whereRange.Last();

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(EmptyRanges))]
    public void Last_ThrowsOnEmptyRange(Range range)
    {
        void Last()
        {
            _ = range.Where(i => i % 2 != 0).Last();
        }

        Assert.Throws<InvalidOperationException>(Last);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void LastOrDefault_MatchesIEnumerableLastOrDefault(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.Where(i => i % 2 != 0);
        var actual = range.Where(i => i % 2 != 0);

        Assert.Equal(expected.LastOrDefault(), actual.LastOrDefault());
#if NET6_0_OR_GREATER
        Assert.Equal(expected.LastOrDefault(42), actual.LastOrDefault(42));
#endif
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Reverse_MatchesIEnumerableReverse(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.Where(i => i % 2 != 0).Reverse();
        var actual = range.Where(i => i % 2 != 0).Reverse();

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void WhereTwice_MatchesIEnumerableWhereTwice(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable
            .Where(i =>
                i % 2 is 0 &&
                i % 3 is 0);

        var actual = range
            .Where(i => i % 2 is 0)
            .Where(i => i % 3 is 0);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void WhereTwice_ThrowsOnNullPredicate()
    {
        static void WhereNull()
        {
            _ = (0..100)
                .Where(i => i % 2 is 0)
                .Where(null!);
        }

        Assert.Throws<ArgumentNullException>(WhereNull);
    }
}
