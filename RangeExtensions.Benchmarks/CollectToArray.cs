using BenchmarkDotNet.Attributes;

namespace RangeExtensions.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 5, exportCombinedDisassemblyReport: true)]
public class CollectToArray
{
    [Params(10, 1000, 1000000)]
    public int Length;

    [Benchmark(Baseline = true)]
    public int[] RangeToArray()
    {
        return (0..Length).AsEnumerable().ToArray();
    }

    [Benchmark]
    public int[] EnumerableToArray()
    {
        return Enumerable.Range(0, Length).ToArray();
    }
}
