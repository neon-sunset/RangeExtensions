using Xunit.Sdk;

namespace RangeExtensions.Tests;

public class RangeEnumerableExtensions
{
    public static IEnumerable<object[]> ValidRangePairs() => Data.ValidRangePairs();
    public static IEnumerable<object[]> EmptyRanges() => Data.EmptyRanges();
    public static IEnumerable<int> Numbers(Range range) =>
        new[]
        {
            0, 1, -1, 7, -7, 10, 1001, 178000, int.MinValue, int.MaxValue,
            range.Start.Value, range.Start.Value + 1, range.Start.Value - 1,
            range.End.Value, range.End.Value + 1, range.End.Value - 1
        };

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Any_MatchesIEnumerableAny(Range range, IEnumerable<int> enumerable)
    {
        var anyRange = range.AsEnumerable().Any();
        var anyEnumerable = enumerable.Any();

        Assert.Equal(anyEnumerable, anyRange);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Average_MatchesIEnumerableAverage(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count is 0)
        {
            return;
        }

        var rangeAverage = rangeEnumerable.Average();
        var enumerableAverage = enumerable.Average();

        Assert.Equal(enumerableAverage, rangeAverage);
    }

    [Theory]
    [MemberData(nameof(EmptyRanges))]
    public void Average_ThrowsOnEmptyRange(Range range)
    {
        void Average()
        {
            _ = range.AsEnumerable().Average();
        }

        Assert.Throws<InvalidOperationException>(Average);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Contains_MatchesIEnumerableContains(Range range, IEnumerable<int> enumerable)
    {
        var numbers = Numbers(range);

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
        if (rangeEnumerable.Count is 0)
        {
            return;
        }

        var rangeFirst = rangeEnumerable.First();
        var enumerableFirst = enumerable.First();

        Assert.Equal(enumerableFirst, rangeFirst);
    }

    [Theory]
    [MemberData(nameof(EmptyRanges))]
    public void First_ThrowsOnEmptyRange(Range range)
    {
        void First()
        {
            _ = range.AsEnumerable().First();
        }

        Assert.Throws<InvalidOperationException>(First);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Last_MatchesIEnumerableLast(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count is 0)
        {
            return;
        }

        var rangeLast = rangeEnumerable.Last();
        var enumerableLast = enumerable.Last();

        Assert.Equal(enumerableLast, rangeLast);
    }

    [Theory]
    [MemberData(nameof(EmptyRanges))]
    public void Last_ThrowsOnEmptyRange(Range range)
    {
        void Last()
        {
            _ = range.AsEnumerable().Last();
        }

        Assert.Throws<InvalidOperationException>(Last);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Max_MatchesIEnumerableMax(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count is 0)
        {
            return;
        }

        var rangeMax = rangeEnumerable.Max();
        var enumerableMax = enumerable.Max();

        Assert.Equal(enumerableMax, rangeMax);
    }

    [Theory]
    [MemberData(nameof(EmptyRanges))]
    public void Max_ThrowsOnEmptyRange(Range range)
    {
        void Max()
        {
            _ = range.AsEnumerable().Max();
        }

        Assert.Throws<InvalidOperationException>(Max);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Min_MatchesIEnumerableMin(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        if (rangeEnumerable.Count is 0)
        {
            return;
        }

        var rangeMin = rangeEnumerable.Min();
        var enumerableMin = enumerable.Min();

        Assert.Equal(enumerableMin, rangeMin);
    }

    [Theory]
    [MemberData(nameof(EmptyRanges))]
    public void Min_ThrowsOnEmptyRange(Range range)
    {
        void Min()
        {
            _ = range.AsEnumerable().Min();
        }

        Assert.Throws<InvalidOperationException>(Min);
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
        var numbers = Numbers(range);

        var rangeResults = numbers.Select(i => (IEnumerable<int>)range.AsEnumerable().Skip(i));
        var enumerableResults = numbers.Select(i => enumerable.Skip(i));

        Assert.Equal(enumerableResults, rangeResults);
    }

    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Sum_MatchesIEnumerableSum(Range range, IEnumerable<int> enumerable)
    {
        int Sum() => range.AsEnumerable().Sum();
        int SumEnumerable() => enumerable.Sum();

        try
        {
            Assert.Equal(SumEnumerable(), Sum());
        }
        catch (Exception expectedException)
        {
            if (expectedException is EqualException)
            {
                throw;
            }

            Assert.Throws(expectedException.GetType(), () => Sum());
            Assert.Throws(expectedException.GetType(), () => SumEnumerable());
        }
    }

    [Fact]
    public void Sum_ThrowsOnOverflow()
    {
        static void Sum()
        {
            _ = (0..65537).AsEnumerable().Sum();
        }

        Assert.Throws<OverflowException>(Sum);
    }


    [Theory]
    [MemberData(nameof(ValidRangePairs))]
    public void Take_MatchesIEnumerableTake(Range range, IEnumerable<int> enumerable)
    {
        var numbers = Numbers(range);

        var rangeResults = numbers.Select(i => (IEnumerable<int>)range.AsEnumerable().Take(i));
        var enumerableResults = numbers.Select(i => enumerable.Take(i));

        Assert.Equal(enumerableResults, rangeResults);
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
