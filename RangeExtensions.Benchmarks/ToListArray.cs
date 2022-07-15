using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace RangeExtensions.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 5, exportCombinedDisassemblyReport: true)]
public class ToListArray
{
    [Params(10, 1000, 1000000)]
    public int Length;

    [Benchmark(Baseline = true)]
    public int[] RangeToArray()
    {
        return (0..Length).ToArray();
    }

    [Benchmark]
    public List<int> RangeToList()
    {
        return (0..Length).ToList();
    }

    [Benchmark]
    public int[] EnumerableToArray()
    {
        return Enumerable.Range(0, Length).ToArray();
    }

    [Benchmark]
    public List<int> EnumerableToList()
    {
        return Enumerable.Range(0, Length).ToList();
    }
}
