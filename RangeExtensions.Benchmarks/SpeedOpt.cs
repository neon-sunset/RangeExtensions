using BenchmarkDotNet.Attributes;

namespace RangeExtensions.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class SpeedOpt
{
    public const int Length = 1000;

    [Benchmark]
    public long RangeAggregate() => (0..Length)
        .Select(i => (long)i)
        .Aggregate(0L, (acc, i) => acc + i);

    [Benchmark]
    public long EnumerableAggregate() => Enumerable.Range(0, Length)
        .Select(i => (long)i)
        .Aggregate(0L, (acc, i) => acc + i);

    [Benchmark]
    public int RangeWhereLast() => (0..Length)
        .Where(i => i % 64 is 0)
        .Last();

    [Benchmark]
    public int EnumerableWhereLast() => Enumerable
        .Range(0, Length)
        .Last(i => i % 64 is 0);

    [Benchmark]
    public long RangeIndex() => (0..Length)
        .Select(i => (long)i)[Length - 16];

    [Benchmark]
    public long RangeElementAt() => (0..Length)
        .Select(i => (long)i)
        .ElementAt(Length - 16);

    [Benchmark]
    public long EnumerableElementAt() => Enumerable
        .Range(0, Length)
        .Select(i => (long)i)
        .ElementAt(Length - 16);
}
