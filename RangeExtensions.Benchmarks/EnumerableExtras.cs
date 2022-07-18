using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace RangeExtensions.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 5, exportCombinedDisassemblyReport: true)]
public class EnumerableExtras
{
    [Params(1, 10, 1000, 1000000)]
    public int Length;

    [Benchmark] public bool RangeAny() => Range(Length).Any();

    [Benchmark] public bool EnumerableAny() => Enumerable(Length).Any();

    [Benchmark] public bool RangeContains() => Range(Length).Contains(Length / 2);

    [Benchmark] public bool EnumerableContains() => Enumerable(Length).Contains(Length / 2);

    [Benchmark] public int RangeCount() => Range(Length).Count();

    [Benchmark] public int EnumerableCount() => Enumerable(Length).Count();

    [Benchmark] public int RangeMax() => Range(Length).Max();

    [Benchmark] public int EnumerableMax() => Enumerable(Length).Max();

    [Benchmark] public double RangeAverage() => Range(Length).Average();

    [Benchmark] public double EnumerableAverage() => Enumerable(Length).Average();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static RangeEnumerable Range(int length) => 0..length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IEnumerable<int> Enumerable(int length) => System.Linq.Enumerable.Range(0, length);
}
