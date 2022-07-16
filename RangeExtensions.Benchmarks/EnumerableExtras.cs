using BenchmarkDotNet.Attributes;

namespace RangeExtensions.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 5, exportCombinedDisassemblyReport: true)]
public class EnumerableExtras
{
    [Params(10, 1000, 1000000)]
    public int Length;

    [Benchmark]
    public int RangeCount()
    {
        return (0..Length).AsEnumerable().Count();
    }

    [Benchmark]
    public int EnumerableCount()
    {
        return Enumerable.Range(0, Length).Count();
    }

    [Benchmark]
    public int RangeMax()
    {
        return (0..Length).AsEnumerable().Max();
    }

    [Benchmark]
    public int EnumerableMax()
    {
        return Enumerable.Range(0, Length).Max();
    }

    [Benchmark]
    public double RangeAverage()
    {
        return (0..Length).AsEnumerable().Average();
    }

    [Benchmark]
    public double EnumerableAverage()
    {
        return Enumerable.Range(0, Length).Average();
    }
}
