namespace RangeExtensions.Tests;

internal static class Data
{
    public static IEnumerable<object[]> ValidRangePairs()
    {
        yield return new object[] { 0..0, Enumerable.Range(0, 0) };
        yield return new object[] { 100..100, Enumerable.Range(100, 0) };
        yield return new object[] { 65537..65537, Enumerable.Range(65537, 0) };
        yield return new object[] { 0..1, Enumerable.Range(0, 1) };
        yield return new object[] { 0..10, Enumerable.Range(0, 10) };
        yield return new object[] { 0..65536, Enumerable.Range(0, 65536) };
        yield return new object[] { 141469..177013, Enumerable.Range(141469, 177013 - 141469) };
        yield return new object[] { (int.MaxValue - 5)..int.MaxValue, Enumerable.Range(int.MaxValue - 5, 5) };
        yield return new object[] { 1..0, Enumerable.Range(0, 1).Reverse() };
        yield return new object[] { 100..0, Enumerable.Range(0, 100).Reverse() };
        yield return new object[] { 65536..0, Enumerable.Range(0, 65536).Reverse() };
        yield return new object[] { int.MaxValue..(int.MaxValue - 5), Enumerable.Range(int.MaxValue - 5, 5).Reverse() };
    }

    public static IEnumerable<object[]> InvalidRanges()
    {
        yield return new object[] { 0..^0 };
        yield return new object[] { 0..^10 };
        yield return new object[] { ^5..10 };
        yield return new object[] { ^5..^10 };
        yield return new object[] { ^5..^10 };
        yield return new object[] { 0.. };
        yield return new object[] { 10.. };
    }

    public static IEnumerable<object[]> EmptyRanges()
    {
        yield return new object[] { 0..0 };
        yield return new object[] { 1337..1337 };
        yield return new object[] { 100..100 };
        yield return new object[] { 65537..65537 };
        yield return new object[] { int.MaxValue..int.MaxValue };
    }

    public static IEnumerable<int> Numbers(Range range) =>
        new[]
        {
            0, 1, -1, 7, -7, 10, 1001, 178000, int.MinValue, int.MaxValue,
            range.Start.Value, range.Start.Value + 1, range.Start.Value - 1,
            range.End.Value, range.End.Value + 1, range.End.Value - 1
        };

    public static IEnumerable<int> Indexes(ICollection<int> collection)
    {
        yield return 0;
        yield return Math.Min(collection.Count, 1);
        yield return collection.Count / 3;
        yield return collection.Count / 2;
        yield return Math.Max(collection.Count - 1, 0);
        yield return collection.Count;
        yield return collection.Count + 1;
        yield return int.MinValue;
        yield return int.MaxValue;
    }
}
