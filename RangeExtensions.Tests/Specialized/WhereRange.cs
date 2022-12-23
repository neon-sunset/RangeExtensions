namespace RangeExtensions.Tests;

public partial class WhereRangeTests
{
    public static IEnumerable<object[]> ValidRangePairs() => Data.ValidRangePairs();

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void Enumerator_MatchesStandardEnumerable(Range range, IEnumerable<int> enumerable)
    {
        IEnumerable<int> Enumerate()
        {
            foreach (var i in range.Where(i => i % 2 != 0))
            {
                yield return i;
            }
        }

        var expected = enumerable.Where(i => i % 2 != 0);
        var actual = Enumerate();

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(ValidRangePairs))]
    public void EnumeratorBoxed_MatchesStandardEnumerable(Range range, IEnumerable<int> enumerable)
    {
        IEnumerable<int> EnumerateBoxed()
        {
            foreach (var i in (IEnumerable<int>)range.Where(i => i % 2 != 0))
            {
                yield return i;
            }
        }

        var expected = enumerable.Where(i => i % 2 != 0);
        var actual = EnumerateBoxed();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EnumeratorReset_ThrowsNotSupportedException()
    {
        static void ResetEnumerator()
        {
            (0..100).Where(i => i % 2 != 0).GetEnumerator().Reset();
        }

        Assert.Throws<NotSupportedException>(ResetEnumerator);
    }
}
