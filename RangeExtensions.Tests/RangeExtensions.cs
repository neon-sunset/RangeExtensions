﻿#if !NET48
using System.Buffers;
#endif

namespace RangeExtensions.Tests;

public class RangeExtensionsTests
{
    public static IEnumerable<object[]> ValidRangePairs() => Data.ValidRangePairs();

    [Theory, MemberData(nameof(ValidRangePairs))]
    public static void Aggregate_MatchesIEnumerableAggregate(Range range, IEnumerable<int> enumerable)
    {
        AssertHelpers.EqualValueOrException(
            () => enumerable.Aggregate((acc, i) => acc + i),
            () => range.Aggregate((acc, i) => acc + i));
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public static void AggregateWithSeed_MatchesIEnumerableAggregateWithSeed(Range range, IEnumerable<int> enumerable)
    {
        const int seed = 34;

        var expected = enumerable.Aggregate(seed, (acc, i) => acc + i);
        var actual = range.Aggregate(seed, (acc, i) => acc + i);

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public static void Select_MatchesIEnumerableSelect(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.Select(i => $"{i * 2}");
        var actual = range.Select(i => $"{i * 2}");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void Select_ThrowsOnNullSelector()
    {
        static void SelectWithNull()
        {
            _ = (0..100).Select((Func<int, long>)null!);
        }

        Assert.Throws<ArgumentNullException>(SelectWithNull);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public static void Where_MatchesIEnumerableWhere(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.Where(i => i % 7 is 0);
        var actual = range.Where(i => i % 7 is 0);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void Where_ThrowsOnNullPredicate()
    {
        static void WhereWithNull()
        {
            _ = (0..100).Where(null!);
        }

        Assert.Throws<ArgumentNullException>(WhereWithNull);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void ToArray_MatchesIEnumerableToArray(Range range, IEnumerable<int> enumerable)
    {
        var rangeToArray = range.ToArray();
        var enumerableToArray = enumerable.ToArray();

        Assert.Equal(enumerableToArray, rangeToArray);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void ToList_MatchesIEnumerableToList(Range range, IEnumerable<int> enumerable)
    {
        var rangeToList = range.ToList();
        var enumerableToList = enumerable.ToList();

        Assert.Equal(enumerableToList, rangeToList);
    }

#if !NET48
    [Theory, MemberData(nameof(ValidRangePairs))]
    public void CopyToSpan_MatchesIEnumerableToArray(Range range, IEnumerable<int> enumerable)
    {
        var enumerableArray = enumerable.ToArray();
        using var rangeBuffer = Buffer.Create(stackalloc int[1024], enumerableArray.Length);

        range.CopyTo(rangeBuffer);

        Assert.True(rangeBuffer.Span.SequenceEqual(enumerableArray));
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void CopyToSpan_DoesNotRunPastRangeCount(Range range, IEnumerable<int> enumerable)
    {
        var enumerableArray = enumerable.Concat(Enumerable.Repeat(0, 10)).ToArray();
        using var rangeBuffer = Buffer.Create(stackalloc int[1024], enumerableArray.Length);

        rangeBuffer.Span.Clear();
        range.CopyTo(rangeBuffer);

        Assert.True(rangeBuffer.Span.SequenceEqual(enumerableArray));
    }

    [Fact]
    public void CopyToSpan_ThrowsOnRangeLongerThanSpan()
    {
        static void CopyToSpan()
        {
            var span = (stackalloc int[128]);

            (..129).CopyTo(span);
        }

        Assert.Throws<ArgumentException>(CopyToSpan);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void TryCopyToSpan_MatchesIEnumerableToArray(Range range, IEnumerable<int> enumerable)
    {
        var enumerableArray = enumerable.ToArray();
        if (enumerableArray.Length > 0)
        {
            using var insufficientBuffer = Buffer.Create(stackalloc int[1024], enumerableArray.Length - 1);

            Assert.False(range.TryCopyTo(insufficientBuffer));
        }

        using var rangeBuffer = Buffer.Create(stackalloc int[1024], enumerableArray.Length);

        Assert.True(range.TryCopyTo(rangeBuffer));
        Assert.True(rangeBuffer.Span.SequenceEqual(enumerableArray));
    }
#endif
}
