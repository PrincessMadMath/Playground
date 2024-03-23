// See https://aka.ms/new-console-template for more information

using Benchmark;
using Benchmark.Json;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<DeserializationStrategyBenchmark>();