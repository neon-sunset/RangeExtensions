using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace RangeExtensions.Benchmarks;

// [ShortRunJob(RuntimeMoniker.Net48)]
[ShortRunJob]
// [ShortRunJob(RuntimeMoniker.Net70)]
// [ShortRunJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[HideColumns("StdDev", "Gen0", "Alloc Ratio")]
// [DisassemblyDiagnoser(maxDepth: 2, exportCombinedDisassemblyReport: true)]
public class ForEach
{
    [Params(1, 100, 100_000)]
    public int Length;

    [Benchmark(Baseline = true)]
    public int For()
    {
        var ret = 0;
        for (int i = 0; i < Length; i++)
        {
            ret += i;
        }

        return ret;
    }

    [Benchmark]
    public int Range()
    {
        var ret = 0;
        foreach (var i in 0..Length)
        {
            ret += i;
        }

        return ret;
    }

    [Benchmark]
    public int EnumerableRange()
    {
        var ret = 0;
        foreach (var i in Enumerable.Range(0, Length))
        {
            ret += i;
        }

        return ret;
    }

    [Benchmark]
    public int RangeSelect()
    {
        var ret = 0;
        foreach (var i in (..Length).Select(i => i * 2))
        {
            ret += i;
        }

        return ret;
    }

    [Benchmark]
    public int EnumerableSelect()
    {
        var ret = 0;
        foreach (var i in Enumerable.Range(0, Length).Select(i => i * 2))
        {
            ret += i;
        }

        return ret;
    }
}
