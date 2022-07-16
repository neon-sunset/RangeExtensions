namespace RangeExtensions.Tests;

public class RangeEnumerableExtensions
{
    private static IEnumerable<object[]> ValidRangePairs() => Data.ValidRangePairs();
    private static IEnumerable<object[]> InvalidRanges() => Data.InvalidRanges();

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Average_MatchesIEnumerableAverage(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count() is 0)
        {
            return;
        }

        var rangeAverage = rangeEnumerable.Average();
        var enumerableAverage = enumerable.Average();

        Assert.Equal(enumerableAverage, rangeAverage);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Contains_MatchesIEnumerableContains(Range range, IEnumerable<int> enumerable)
    {
        var numbers = new[] { 0, 10, 1001, 178000, int.MaxValue };
        var rangeResults = numbers.Select(i => range.AsEnumerable().Contains(i));
        var enumerableResults = numbers.Select(i => enumerable.Contains(i));

        Assert.Equal(enumerableResults, rangeResults);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Count_MatchesIEnumerableCount(Range range, IEnumerable<int> enumerable)
    {
        var rangeCount = range.AsEnumerable().Count();
        var enumerableCount = enumerable.Count();

        Assert.Equal(enumerableCount, rangeCount);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Distinct_MatchesIEnumerableDistinct(Range range, IEnumerable<int> enumerable)
    {
        var rangeDistinct = range.AsEnumerable().Distinct();
        var enumerableDistinct = enumerable.Distinct();

        Assert.Equal(range.AsEnumerable(), rangeDistinct);
        Assert.Equal(enumerableDistinct, rangeDistinct);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void First_MatchesIEnumerableFirst(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count() is 0)
        {
            return;
        }

        var rangeFirst = rangeEnumerable.First();
        var enumerableFirst = enumerable.First();

        Assert.Equal(enumerableFirst, rangeFirst);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Last_MatchesIEnumerableLast(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count() is 0)
        {
            return;
        }

        var rangeLast = rangeEnumerable.Last();
        var enumerableLast = enumerable.Last();

        Assert.Equal(enumerableLast, rangeLast);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Max_MatchesIEnumerableMax(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count() is 0)
        {
            return;
        }

        var rangeMax = rangeEnumerable.Max();
        var enumerableMax = enumerable.Max();

        Assert.Equal(enumerableMax, rangeMax);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Min_MatchesIEnumerableMin(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count() is 0)
        {
            return;
        }

        var rangeMin = rangeEnumerable.Min();
        var enumerableMin = enumerable.Min();

        Assert.Equal(enumerableMin, rangeMin);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Reverse_MatchesIEnumerableReverse(Range range, IEnumerable<int> enumerable)
    {
        var rangeReverse = range.AsEnumerable().Reverse();
        var enumerableReverse = enumerable.Reverse();

        Assert.Equal(enumerableReverse, rangeReverse);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Skip_MatchesIEnumerableSkip(Range range, IEnumerable<int> enumerable)
    {
        var rangeSkip = range.AsEnumerable().Skip(1);
        var enumerableSkip = enumerable.Skip(1);

        Assert.Equal(enumerableSkip, rangeSkip);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Take_MatchesIEnumerableTake(Range range, IEnumerable<int> enumerable)
    {
        var rangeTake = range.AsEnumerable().Take(1);
        var enumerableTake = enumerable.Take(1);

        Assert.Equal(enumerableTake, rangeTake);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void ToArray_MatchesIEnumerableToArray(Range range, IEnumerable<int> enumerable)
    {
        var rangeToArray = range.ToArray();
        var enumerableToArray = enumerable.ToArray();

        Assert.Equal(enumerableToArray, rangeToArray);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void ToList_MatchesIEnumerableToList(Range range, IEnumerable<int> enumerable)
    {
        var rangeToList = range.ToList();
        var enumerableToList = enumerable.ToList();

        Assert.Equal(enumerableToList, rangeToList);
    }
}
