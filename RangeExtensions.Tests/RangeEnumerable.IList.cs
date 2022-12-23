namespace RangeExtensions.Tests;

public partial class RangeEnumerableTests
{
    [Theory, MemberData(nameof(ValidRangePairs))]
    public void IndexOperator_MatchesIListIndexOperator(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.ToList();
        var actual = range.AsEnumerable();

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void IndexOperator_ThrowsOnOutOfBoundsAccess(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.ToList();
        var actual = range.AsEnumerable();

        foreach (var i in Data.InvalidIndexes(expected))
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => expected[i]);
            Assert.Throws<ArgumentOutOfRangeException>(() => actual[i]);
        }
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void IndexOf_MatchesIListIndexOf(Range range, IEnumerable<int> enumerable)
    {
        var expected = enumerable.ToList();
        var actual = range.AsEnumerable();

        foreach (var value in Data.Indexes(expected))
        {
            Assert.Equal(expected.IndexOf(value), actual.IndexOf(value));
        }
    }

    [Fact]
    public void Insert_ThrowsNotSupportedException()
    {
        static void Insert()
        {
            (..100).AsEnumerable().Insert(10, 1);
        }

        Assert.Throws<NotSupportedException>(Insert);
    }

    [Fact]
    public void RemoveAt_ThrowsNotSupportedException()
    {
        static void RemoveAt()
        {
            (..100).AsEnumerable().RemoveAt(10);
        }

        Assert.Throws<NotSupportedException>(RemoveAt);
    }
}
