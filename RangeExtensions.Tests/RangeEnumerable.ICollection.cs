﻿namespace RangeExtensions.Tests;

public partial class RangeEnumerableTests
{
    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Count_MatchesIEnumerableCount(Range range, IEnumerable<int> enumerable)
    {
        var rangeCount = range.AsEnumerable().Count;
        var enumerableCount = enumerable.Count();

        Assert.Equal(enumerableCount, rangeCount);
    }

    [Fact]
    public void IsReadOnly_ReturnTrue()
    {
        var rangeEnumerable = (..100).AsEnumerable();

        Assert.True(rangeEnumerable.IsReadOnly);
    }

    [Fact]
    public void Add_ThrowsNotSupportedException()
    {
        static void Add()
        {
            (..100).AsEnumerable().Add(10);
        }

        Assert.Throws<NotSupportedException>(Add);
    }

    [Fact]
    public void Clear_ThrowsNotSupportedException()
    {
        static void Clear()
        {
            (..100).AsEnumerable().Clear();
        }

        Assert.Throws<NotSupportedException>(Clear);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Contains_MatchesIEnumerableContains(Range range, IEnumerable<int> enumerable)
    {
        var numbers = Data.Numbers(range);

        var rangeResults = numbers.Select(i => range.AsEnumerable().Contains(i));
        var enumerableResults = numbers.Select(i => enumerable.Contains(i));

        Assert.Equal(enumerableResults, rangeResults);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void CopyTo_MatchesICollectionCopyTo(Range range, IEnumerable<int> enumerable)
    {
        var rangeEnumerable = range.AsEnumerable();
        var numbersArray = enumerable.ToArray();

        static int[] CopyCollection<TCollection>(int index, TCollection collection)
            where TCollection : ICollection<int>
        {
            var numbers = new int[collection.Count];

            collection.CopyTo(numbers, index);

            return numbers;
        }

        foreach (var index in Data.Indexes(numbersArray))
        {
            AssertHelpers.EqualSequenceOrException(
                () => CopyCollection(index, numbersArray),
                () => CopyCollection(index, rangeEnumerable),
                allowInherited: true);
        }
    }

#if !NET48
    [Theory, MemberData(nameof(ValidRangePairs))]
    public void CopyToSpan_MatchesICollectionCopyTo(Range range, IEnumerable<int> enumerable)
    {
        var numbersArray = enumerable.ToArray();

        static int[] CopyCollection<TCollection>(int index, TCollection collection)
            where TCollection : ICollection<int>
        {
            var numbers = new int[collection.Count];

            collection.CopyTo(numbers, index);

            return numbers;
        }

        static int[] CopySpan(int index, RangeEnumerable rangeCollection)
        {
            var numbers = new int[rangeCollection.Count];

            rangeCollection.CopyTo(numbers.AsSpan(), index);

            return numbers;
        }

        foreach (var index in Data.Indexes(numbersArray))
        {
            AssertHelpers.EqualSequenceOrException(
                () => CopyCollection(index, numbersArray),
                () => CopySpan(index, range),
                allowInherited: true);
        }
    }
#endif

    [Fact]
    public void Remove_ThrowsNotSupportedException()
    {
        static void Remove()
        {
            _ = (..100).AsEnumerable().Remove(10);
        }

        Assert.Throws<NotSupportedException>(Remove);
    }
}
