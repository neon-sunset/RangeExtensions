namespace RangeExtensions.Tests;

public partial class SelectRangeTests
{
    public static IEnumerable<object[]> ValidRangePairs() => Data.ValidRangePairs();

    [Theory, MemberData(nameof(Data.ValidRangePairs))]
    public void Count_MatchesIListCount(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.Select(i => i).ToList().Count;
        var actual = range.Select(i => i).ToList().Count;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsReadOnly_ReturnsTrue()
    {
        var selectRange = (0..100).Select(i => i);

        Assert.True(selectRange.IsReadOnly);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void IndexOperator_MatchesIListIndexOperator(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.ToList();
        var actual = range.Select(i => i);

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void IndexOperator_ThrowsOnOutOfBoundsAccess(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.ToList();
        var actual = range.Select(i => i);

        foreach (var i in Data.InvalidIndexes(expected))
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => expected[i]);
            Assert.Throws<ArgumentOutOfRangeException>(() => actual[i]);
        }
    }

    [Fact]
    public void IndexOperator_SetThrowsNotSupportedException()
    {
        static void SetIndex()
        {
            (0..100).Select(i => i)[10] = 10;
        }

        Assert.Throws<NotSupportedException>(SetIndex);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Contains_MatchesIEnumerableContains(Range range, IEnumerable<int> enumerable)
    {
        var numbers = Data.Numbers(range);

        var selectRangeResults = numbers.Select(i => range.Select(j => j).Contains(i));
        var enumerableResults = numbers.Select(i => enumerable.Contains(i));

        Assert.Equal(enumerableResults, selectRangeResults);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void CopyTo_MatchesICollectionCopyTo(Range range, IEnumerable<int> enumerable)
    {
        var selectRangeEnumerable = range.Select(i => i);
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
                () => CopyCollection(index, selectRangeEnumerable),
                allowInherited: true);
        }
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void IndexOf_MatchesIListIndexOf(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.ToList();
        var actual = range.Select(i => i);

        foreach (var value in Data.Indexes(expected))
        {
            Assert.Equal(expected.IndexOf(value), actual.IndexOf(value));
        }
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Enumerator_MatchesStandardEnumerable(Range range, IEnumerable<int> enumerable)
    {
        IEnumerable<int> Enumerate()
        {
            foreach (var i in range.Select(i => i))
            {
                yield return i;
            }
        }

        Assert.Equal(enumerable, Enumerate());
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void EnumeratorBoxed_MatchesStandardEnumerable(Range range, IEnumerable<int> enumerable)
    {
        IEnumerable<int> EnumerateBoxed()
        {
            foreach (var i in (IEnumerable<int>)range.Select(i => i))
            {
                yield return i;
            }
        }

        Assert.Equal(enumerable, EnumerateBoxed());
    }

    [Fact]
    public void EnumeratorReset_ThrowsNotSupportedException()
    {
        static void ResetEnumerator()
        {
            (0..100).Select(i => i).GetEnumerator().Reset();
        }

        Assert.Throws<NotSupportedException>(ResetEnumerator);
    }

    [Fact]
    public void Add_ThrowsNotSupportedException()
    {
        static void Add()
        {
            (0..100).Select(i => i).Add(10);
        }

        Assert.Throws<NotSupportedException>(Add);
    }

    [Fact]
    public void Clear_ThrowsNotSupportedException()
    {
        static void Clear()
        {
            (0..100).Select(i => i).Clear();
        }

        Assert.Throws<NotSupportedException>(Clear);
    }

    [Fact]
    public void Insert_ThrowsNotSupportedException()
    {
        static void Insert()
        {
            (0..100).Select(i => i).Insert(10, 10);
        }

        Assert.Throws<NotSupportedException>(Insert);
    }

    [Fact]
    public void Remove_ThrowsNotSupportedException()
    {
        static void Remove()
        {
            (0..100).Select(i => i).Remove(10);
        }

        Assert.Throws<NotSupportedException>(Remove);
    }

    [Fact]
    public void RemoveAt_ThrowsNotSupportedException()
    {
        static void RemoveAt()
        {
            (0..100).Select(i => i).RemoveAt(10);
        }

        Assert.Throws<NotSupportedException>(RemoveAt);
    }
}
