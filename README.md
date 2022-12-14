# RangeExtensions
[![CI/CD](https://github.com/neon-sunset/RangeExtensions/actions/workflows/dotnet-releaser.yml/badge.svg)](https://github.com/neon-sunset/RangeExtensions/actions/workflows/dotnet-releaser.yml) [![nuget](https://badgen.net/nuget/v/RangeExtensions/latest)](https://www.nuget.org/packages/RangeExtensions/) [![Coverage Status](https://coveralls.io/repos/github/neon-sunset/RangeExtensions/badge.svg)](https://coveralls.io/github/neon-sunset/RangeExtensions)

This package enables the usage of `System.Range` in `foreach` expressions and provides optimized extensions to integrate `Range` with LINQ.

- Correctness is verified against standard `IEnumerable<int>` and `Enumerable.Range` behavior;
- The library tries its best to make the abstractions either zero-cost or near zero-cost. For critical paths, performance is hand tuned to be allocation-free and on par with regular `for` loops

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
var floats = (0..100)
    .Select(i => (float)i)
    .ToArray();


var even = (0..100).Where(i => i % 2 == 0);
```

### Collecting to array or list
```cs
var numbers = (0..100).ToArray();
```

### Specialized implementations
```cs
var enumerable = (..100).AsEnumerable();

var sum = enumerable.Sum();
var count = enumerable.Count();
var average = enumerable.Average();
var firstTen = enumerable.Take(10);
var reversed = enumerable.Reverse();
// and others
```

## Performance
In short: 10x fast vs `Enumerable.Range()` and as fast as a plain `for` loop (there's small fixed overhead to check range correctness).
``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 5800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.100-rc.1.22363.32
  [Host]   : .NET 7.0.0 (7.0.22.36203), X64 RyuJIT
  ShortRun : .NET 7.0.0 (7.0.22.36203), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|            Method |   Length |             Mean |          Error |        StdDev | Ratio | RatioSD | Code Size |  Gen 0 | Allocated |
|------------------ |--------- |-----------------:|---------------:|--------------:|------:|--------:|----------:|-------:|----------:|
|               **For** |      **100** |         **23.94 ns** |       **0.556 ns** |      **0.030 ns** |  **1.00** |    **0.00** |      **20 B** |      **-** |         **-** |
|             Range |      100 |         24.91 ns |       0.138 ns |      0.008 ns |  1.04 |    0.00 |      65 B |      - |         - |
|      RangeReverse |      100 |         27.40 ns |       0.245 ns |      0.013 ns |  1.14 |    0.00 |      65 B |      - |         - |
|  Enumerable.Range |      100 |        269.46 ns |      52.032 ns |      2.852 ns | 11.25 |    0.13 |     322 B | 0.0024 |      40 B |
|Range.AsEnumerable |      100 |         24.92 ns |       0.522 ns |      0.029 ns |  1.04 |    0.00 |      67 B |      - |         - |
|                   |          |                  |                |               |       |         |           |        |           |
|               **For** |    **10000** |      **2,085.24 ns** |     **300.295 ns** |     **16.460 ns** |  **1.00** |    **0.00** |      **20 B** |      **-** |         **-** |
|             Range |    10000 |      2,085.39 ns |     308.278 ns |     16.898 ns |  1.00 |    0.00 |      65 B |      - |         - |
|      RangeReverse |    10000 |      2,078.58 ns |      81.149 ns |      4.448 ns |  1.00 |    0.01 |      65 B |      - |         - |
|  Enumerable.Range |    10000 |     27,364.70 ns |     616.148 ns |     33.773 ns | 13.12 |    0.11 |     322 B |      - |      40 B |
|Range.AsEnumerable |    10000 |      2,104.25 ns |     464.044 ns |     25.436 ns |  1.01 |    0.01 |      67 B |      - |         - |
|                   |          |                  |                |               |       |         |           |        |           |
|               **For** | **10000000** |  **2,086,119.92 ns** | **289,496.016 ns** | **15,868.253 ns** |  **1.00** |    **0.00** |      **20 B** |      **-** |         **-** |
|             Range | 10000000 |  2,086,358.07 ns | 335,673.174 ns | 18,399.379 ns |  1.00 |    0.02 |      65 B |      - |         - |
|      RangeReverse | 10000000 |  2,083,810.55 ns | 342,667.388 ns | 18,782.756 ns |  1.00 |    0.01 |      65 B |      - |         - |
|  Enumerable.Range | 10000000 | 27,263,256.25 ns | 396,121.214 ns | 21,712.740 ns | 13.07 |    0.09 |     322 B |      - |         - |
|Range.AsEnumerable | 10000000 |  2,075,666.41 ns |  45,777.672 ns |  2,509.229 ns |  1.00 |    0.01 |      67 B |      - |         - |

