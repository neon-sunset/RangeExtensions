//using BenchmarkDotNet.Running;

//BenchmarkSwitcher
//   .FromAssembly(typeof(Program).Assembly)
//   .Run();

foreach (var i in (0..10).AsEnumerable().Reverse())
{
    Console.WriteLine(i);
}