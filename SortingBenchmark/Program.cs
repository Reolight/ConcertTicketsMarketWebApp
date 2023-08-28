// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using SortingBenchmark;

BenchmarkRunner.Run<Benchmark>();
Console.ReadKey();