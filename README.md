# RangeExtensions
[![CI/CD](https://github.com/neon-sunset/RangeExtensions/actions/workflows/dotnet-releaser.yml/badge.svg)](https://github.com/neon-sunset/RangeExtensions/actions/workflows/dotnet-releaser.yml) [![nuget](https://badgen.net/nuget/v/RangeExtensions/latest)](https://www.nuget.org/packages/RangeExtensions/) [![Coverage Status](https://coveralls.io/repos/github/neon-sunset/RangeExtensions/badge.svg)](https://coveralls.io/github/neon-sunset/RangeExtensions)

This package enables the usage of `System.Range` in `foreach` expressions and provides extensions to integrate it with LINQ as a faster replacement to `Enumerable.Range`.

- Correctness is verified against `IEnumerable<int>` and `Enumerable.Range` behavior;
- Implementation tries its best to make abstractions either zero-cost or reasonably close to that. For critical paths, performance is tuned to be allocation-free and on par with regular `for` loops

## Features
### Range enumeration
```cs
foreach (var i in ..100) // you can write 0..Length as just ..Length
{
    Console.WriteLine(i);
}
```

### Reverse range enumeration
```cs
for (var i = 100 - 1; i >= 0; i--)
{
    Console.WriteLine(i);
}

// Can be written as
foreach (var i in 100..0)
{
    Console.WriteLine(i);
}
```

### Select and Where
```cs
var floats = (0..100).Select(i => (float)i);
var odd = (0..100).Where(i => i % 2 != 0);

var randomNumbers = (0..1000)
    .Select(_ => Random.Shared.Next())
    .ToArray();
```

### Collecting to array or list
```cs
var numbers = (0..100).ToArray();
```

### Aggregate
```cs
var digits = (0..10)
    .Aggregate(new StringBuilder(), (sb, i) => sb.Append(i))
    .ToString();

Assert.Equal("0123456789", digits);
```

### Other LINQ specializations
```cs
var enumerable = (..100).AsEnumerable();

var sum = enumerable.Sum();
var count = enumerable.Count;
var average = enumerable.Average();
var firstTen = enumerable.Take(10);
var reversed = enumerable.Reverse();
// and others
```

## Performance
In .NET 7, `foreach (var i in 0..Length)` has the same performance as `for` loop. Otherwise, `RangeExtensions` is 2 to >10 times faster than `Enumerable.Range`. Using DynamicPGO significantly improves the performance of both.
``` ini
BenchmarkDotNet=v0.13.2, OS=macOS 13.1 (22C65) [Darwin 22.2.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK=8.0.100-alpha.1.22620.11
  [Host]     : .NET 7.0.1 (7.0.122.56804), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 7.0.1 (7.0.122.56804), Arm64 RyuJIT AdvSIMD

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3

DOTNET_TieredPGO=1
DOTNET_ReadyToRun=0
```
|                Method | Length |            Mean |         Error | Ratio | Allocated |
|---------------------- |------- |----------------:|--------------:|------:|----------:|
|                   For |      1 |       0.0000 ns |     0.0000 ns |     ? |         - |
|                 **Range** |      1 |       0.6531 ns |     0.0051 ns |     ? |         - |
|       EnumerableRange |      1 |       8.1135 ns |     0.0198 ns |     ? |      40 B |
|           **RangeSelect** |      1 |       1.1588 ns |     0.0048 ns |     ? |         - |
|      EnumerableSelect |      1 |      40.7948 ns |     0.7697 ns |     ? |      88 B |
|      **RangeSelectTwice** |      1 |      19.4165 ns |     0.0480 ns |     ? |      96 B |
| EnumerableSelectTwice |      1 |      43.6399 ns |     0.0908 ns |     ? |     232 B |
|            **RangeWhere** |      1 |       1.3954 ns |     0.0036 ns |     ? |         - |
|       EnumerableWhere |      1 |      26.1945 ns |     0.0534 ns |     ? |      96 B |
|                       |        |                 |               |       |           |
|                   For |    100 |      36.1897 ns |     0.0618 ns |  1.00 |         - |
|                 **Range** |    100 |      36.9244 ns |     2.0789 ns |  1.00 |         - |
|       EnumerableRange |    100 |     211.4774 ns |     0.6430 ns |  5.85 |      40 B |
|           **RangeSelect** |    100 |      42.6852 ns |     0.1689 ns |  1.18 |         - |
|      EnumerableSelect |    100 |     235.7174 ns |     0.5161 ns |  6.51 |      88 B |
|      **RangeSelectTwice** |    100 |     110.1340 ns |     0.1667 ns |  3.04 |      96 B |
| EnumerableSelectTwice |    100 |     298.2976 ns |     2.7831 ns |  8.22 |     232 B |
|            **RangeWhere** |    100 |      56.1455 ns |     0.1701 ns |  1.55 |         - |
|       EnumerableWhere |    100 |     249.1264 ns |     1.5890 ns |  6.89 |      96 B |
|                       |        |                 |               |       |           |
|                   For | 100000 |  31,167.5682 ns |    37.3266 ns |  1.00 |         - |
|                 **Range** | 100000 |  31,173.8688 ns |    13.0666 ns |  1.00 |         - |
|       EnumerableRange | 100000 | 212,925.2827 ns |   156.8182 ns |  6.83 |      40 B |
|           **RangeSelect** | 100000 |  50,086.3342 ns |    39.0657 ns |  1.61 |         - |
|      EnumerableSelect | 100000 | 204,113.5813 ns |   100.0221 ns |  6.55 |      88 B |
|      **RangeSelectTwice** | 100000 |  94,302.1444 ns |   230.6254 ns |  3.02 |      96 B |
| EnumerableSelectTwice | 100000 | 203,946.9247 ns |   908.5243 ns |  6.56 |     232 B |
|            **RangeWhere** | 100000 |  47,165.0569 ns |    36.7208 ns |  1.51 |         - |
|       EnumerableWhere | 100000 | 209,918.1519 ns | 3,298.4418 ns |  6.76 |      96 B |

More details on performance: [Releases](https://github.com/neon-sunset/RangeExtensions/releases) tab contains notes on improvements introduced with each version.
