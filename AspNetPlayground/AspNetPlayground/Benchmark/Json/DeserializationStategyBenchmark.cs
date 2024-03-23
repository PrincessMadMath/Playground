using System.Net.Http.Json;
using System.Text.Json;
using Benchmark.Json.Models;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Json;

// Source generation OR reflection based deserialization
// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
// https://www.youtube.com/watch?v=HhyBaJ7uisU

/*
| Method                               | Count | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------------- |------ |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| GetStringThenDeserialize             | 5     |  1.603 ms | 0.0444 ms | 0.0294 ms |  1.00 |    0.00 |        - |        - |        - |     7.6 KB |        1.00 |
| GetStreamThenDeserialize             | 5     |  1.584 ms | 0.0434 ms | 0.0227 ms |  0.99 |    0.02 |        - |        - |        - |    5.38 KB |        0.71 |
| GetFromJsonAsync                     | 5     |  1.610 ms | 0.0770 ms | 0.0458 ms |  1.01 |    0.03 |        - |        - |        - |    6.15 KB |        0.81 |
| GetFromJsonAsyncWithSourceGeneration | 5     |  1.600 ms | 0.0308 ms | 0.0204 ms |  1.00 |    0.03 |        - |        - |        - |    6.17 KB |        0.81 |
|                                      |       |           |           |           |       |         |          |          |          |            |             |
| GetStringThenDeserialize             | 10000 | 54.984 ms | 1.9366 ms | 1.2809 ms |  1.00 |    0.00 | 777.7778 | 444.4444 | 111.1111 | 9702.98 KB |        1.00 |
| GetStreamThenDeserialize             | 10000 | 49.601 ms | 1.4457 ms | 0.9563 ms |  0.90 |    0.03 | 777.7778 | 444.4444 | 111.1111 | 4324.12 KB |        0.45 |
| GetFromJsonAsync                     | 10000 | 49.318 ms | 1.7600 ms | 1.1641 ms |  0.90 |    0.03 | 727.2727 | 363.6364 |  90.9091 | 4323.96 KB |        0.45 |
| GetFromJsonAsyncWithSourceGeneration | 10000 | 49.462 ms | 1.4832 ms | 0.9810 ms |  0.90 |    0.02 | 777.7778 | 444.4444 | 111.1111 | 4324.26 KB |        0.45 |

Not sure of quality of benchmark since first deserialization has been Jitted I think
 */

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 5, iterationCount: 10)]
public class DeserializationStrategyBenchmark
{
    private HttpClient _httpClient = null!;
    
    [Params( 5, 10000)]
    public int Count = 10;
    
    [GlobalSetup]
    public void Setup()
    {
        this._httpClient = new HttpClient();
        this._httpClient.BaseAddress = new Uri("https://localhost:7163/");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _httpClient.Dispose();
    }
    
    private string QueryUri => $"Benchmark/content?count={this.Count}";
    
    [Benchmark(Baseline = true)]
    public async Task GetStringThenDeserialize()
    {
        var stringContent = await this._httpClient.GetStringAsync(QueryUri);
        var result = JsonSerializer.Deserialize<BenchmarkContent>(stringContent);
    }
    
    [Benchmark]
    public async Task GetStreamThenDeserialize()
    {
        var stream = await this._httpClient.GetStreamAsync(QueryUri);
        var result = await JsonSerializer.DeserializeAsync<BenchmarkContent>(stream);
    }
    
    [Benchmark]
    public async Task GetFromJsonAsync()
    {
        var result = await this._httpClient.GetFromJsonAsync<BenchmarkContent>(QueryUri);
    }
    
    [Benchmark]
    public async Task GetFromJsonAsyncWithSourceGeneration()
    {
        // Will deserialize the response as it is being read
        var result = await this._httpClient.GetFromJsonAsync(QueryUri, BenchmarkEntryContentGenerationContext.Default.BenchmarkContent);
    }
}