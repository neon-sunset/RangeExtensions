using BenchmarkDotNet.Attributes;

namespace RangeExtensions.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 5, exportCombinedDisassemblyReport: true)]
public class ForEach
{
    [Params(10, 1000, 10000000)]
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
    public int RangeReverse()
    {
        var ret = 0;
        foreach (var i in Length..0)
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
    public int RangeAsEnumerable()
    {
        var ret = 0;
        foreach (var i in (0..Length).AsEnumerable())
        {
            ret += i;
        }

        return ret;
    }
}
