using BenchmarkDotNet.Attributes;

namespace RangeExtensions.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class SpeedOpt
{
    [Params(10, 1000)] public int Length;

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
        .Where(i => i % 17 is 0)
        .Last();
    
    [Benchmark]
    public int EnumerableWhereLast() => Enumerable.Range(0, Length)
        .Where(i => i % 17 is 0)
        .Last();
}