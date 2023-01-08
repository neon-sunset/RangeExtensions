using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace RangeExtensions.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 5, exportCombinedDisassemblyReport: true)]
public class ToListArray
{
    [Params(10, 1000, 100000)]
    public int Length;

    [Benchmark(Baseline = true)]
    public int[] RangeToArray() => (0..Length).ToArray();

    [Benchmark]
    public int[] EnumerableToArray() => Enumerable.Range(0, Length).ToArray();

    [Benchmark]
    public List<int> RangeToList() => (0..Length).ToList();

    [Benchmark]
    public List<int> EnumerableToList() => Enumerable.Range(0, Length).ToList();

    [Benchmark]
    public int[] RangeSelectToArray() => (0..Length).Select(i => i).ToArray();

    [Benchmark]
    public int[] EnumerableSelectToArray() => Enumerable.Range(0, Length).Select(i => i).ToArray();
}
