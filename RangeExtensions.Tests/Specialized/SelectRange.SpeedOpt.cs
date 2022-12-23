namespace RangeExtensions.Tests;

public partial class SelectRangeTests
{
    public static IEnumerable<object[]> EmptyRanges() => Data.EmptyRanges();

    [Theory, MemberData(nameof(ValidRangePairs))]
    public static void Aggregate_MatchesIEnumerableAggregate(Range range, IEnumerable<int> enumerable)
    {
        AssertHelpers.EqualValueOrException(
            () => enumerable.Aggregate((acc, i) => acc + i),
            () => range.Select(i => i).Aggregate((acc, i) => acc + i));
    }

    [Fact]
    public static void Aggregate_ThrowsOnNullDelegate()
    {
        static void AggregateNull()
        {
            _ = (0..100).Select(i => i).Aggregate(null!);
        }

        Assert.Throws<ArgumentNullException>(AggregateNull);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public static void AggregateWithSeed_MatchesIEnumerableAggregateWithSeed(Range range, IEnumerable<int> enumerable)
    {
        const int seed = 34;

        var expected = enumerable.Aggregate(seed, (acc, i) => acc + i);
        var actual = range.Select(i => i).Aggregate(seed, (acc, i) => acc + i);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void AggregateWithSeed_ThrowsOnNullDelegate()
    {
        static void AggregateNull()
        {
            _ = (0..100).Select(i => i).Aggregate(34, (Func<long, int, long>)null!);
        }

        Assert.Throws<ArgumentNullException>(AggregateNull);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Any_MatchesIEnumerableAny(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.Any();
        var actual = range.Select(i => i).Any();

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void First_MatchesIEnumerableFirst(Range range, IEnumerable<int> enumerable)
    {
        var selectRange = range.Select(i => i);
        if (selectRange.Count is 0)
        {
            return;
        }

        var expected = enumerable.First();
        var actual = selectRange.First();

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(EmptyRanges))]
    public void First_ThrowsOnEmptyRange(Range range)
    {
        void First()
        {
            _ = range.Select(i => i).First();
        }

        Assert.Throws<InvalidOperationException>(First);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void FirstOrDefault_MatchesIEnumerableFirstOrDefault(Range range, IEnumerable<int> enumerable)
    {
        var select = range.Select(i => i);

        Assert.Equal(enumerable.FirstOrDefault(), select.FirstOrDefault());
#if NET6_0_OR_GREATER
        Assert.Equal(enumerable.FirstOrDefault(42), select.FirstOrDefault(42));
#endif
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Last_MatchesIEnumerableLast(Range range, IEnumerable<int> enumerable)
    {
        var selectRange = range.Select(i => i);
        if (selectRange.Count is 0)
        {
            return;
        }

        var expected = enumerable.Last();
        var actual = selectRange.Last();

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(EmptyRanges))]
    public void Last_ThrowsOnEmptyRange(Range range)
    {
        void Last()
        {
            _ = range.Select(i => i).Last();
        }

        Assert.Throws<InvalidOperationException>(Last);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void LastOrDefault_MatchesIEnumerableLastOrDefault(Range range, IEnumerable<int> enumerable)
    {
        var select = range.Select(i => i);

        Assert.Equal(enumerable.LastOrDefault(), select.LastOrDefault());
#if NET6_0_OR_GREATER
        Assert.Equal(enumerable.LastOrDefault(42), select.LastOrDefault(42));
#endif
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Reverse_MatchesIEnumerableReverse(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.Reverse();
        var actual = range.Select(i => i).Reverse();

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void SelectTwice_MatchesIEnumerableSelectTwice(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable
            .Select(i => (long)i)
            .Select(i => i * 2);

        var actual = range
            .Select(i => (long)i)
            .Select(i => i * 2);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectTwice_ThrowsOnNullSelector()
    {
        static void SelectNull()
        {
            _ = (0..100)
                .Select(i => i)
                .Select((Func<int, long>)null!);
        }

        Assert.Throws<ArgumentNullException>(SelectNull);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Skip_MatchesIEnumerableSkip(Range range, IEnumerable<int> enumerable)
    {
        var numbers = Data.Numbers(range);

        var expected = numbers.Select(i => enumerable.Skip(i));
        var actual = numbers.Select(i => (IEnumerable<int>)range.Select(i => i).Skip(i));

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Take_MatchesIEnumerableTake(Range range, IEnumerable<int> enumerable)
    {
        var numbers = Data.Numbers(range);

        var expected = numbers.Select(i => enumerable.Take(i));
        var actual = numbers.Select(i => (IEnumerable<int>)range.Select(i => i).Take(i));

        Assert.Equal(expected, actual);
    }
}
