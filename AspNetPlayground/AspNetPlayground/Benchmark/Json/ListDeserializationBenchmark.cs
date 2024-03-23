using System.Net.Http.Json;
using System.Text.Json;
using Benchmark.Json.Models;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Json;

/*
| Method                   | Count | Path                 | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------- |------ |--------------------- |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| GetStringThenDeserialize | 5     | async(...)items [22] |  1.504 ms | 0.0655 ms | 0.0389 ms |  1.00 |    0.00 |        - |        - |        - |    7.69 KB |        1.00 |
| GetFromJsonAsync         | 5     | async(...)items [22] |  1.509 ms | 0.0534 ms | 0.0353 ms |  1.00 |    0.04 |        - |        - |        - |    6.32 KB |        0.82 |
| GetFromJsonAsAsyncEnumer | 5     | async(...)items [22] |  1.504 ms | 0.0614 ms | 0.0406 ms |  1.00 |    0.04 |        - |        - |        - |    6.71 KB |        0.87 |
|                          |       |                      |           |           |           |       |         |          |          |          |            |             |
| GetStringThenDeserialize | 5     | enumerable-items     |  1.496 ms | 0.0586 ms | 0.0388 ms |  1.00 |    0.00 |        - |        - |        - |    7.65 KB |        1.00 |
| GetFromJsonAsync         | 5     | enumerable-items     |  1.477 ms | 0.0155 ms | 0.0081 ms |  0.98 |    0.03 |        - |        - |        - |    6.28 KB |        0.82 |
| GetFromJsonAsAsyncEnumer | 5     | enumerable-items     |  1.490 ms | 0.0440 ms | 0.0262 ms |  0.99 |    0.03 |        - |        - |        - |    6.62 KB |        0.87 |
|                          |       |                      |           |           |           |       |         |          |          |          |            |             |
| GetStringThenDeserialize | 5     | list-items           |  1.490 ms | 0.0335 ms | 0.0222 ms |  1.00 |    0.00 |        - |        - |        - |    7.58 KB |        1.00 |
| GetFromJsonAsync         | 5     | list-items           |  1.512 ms | 0.0744 ms | 0.0492 ms |  1.01 |    0.03 |        - |        - |        - |     6.2 KB |        0.82 |
| GetFromJsonAsAsyncEnumer | 5     | list-items           |  1.546 ms | 0.0469 ms | 0.0279 ms |  1.04 |    0.02 |        - |        - |        - |    6.59 KB |        0.87 |
|                          |       |                      |           |           |           |       |         |          |          |          |            |             |
| GetStringThenDeserialize | 10000 | async(...)items [22] | 57.964 ms | 4.0014 ms | 2.0928 ms |  1.00 |    0.00 | 750.0000 | 375.0000 | 125.0000 | 9791.09 KB |        1.00 |
| GetFromJsonAsync         | 10000 | async(...)items [22] | 47.837 ms | 5.4384 ms | 3.5972 ms |  0.84 |    0.07 | 833.3333 | 500.0000 | 166.6667 | 4466.83 KB |        0.46 |
| GetFromJsonAsAsyncEnumer | 10000 | async(...)items [22] | 42.435 ms | 1.2106 ms | 0.8007 ms |  0.73 |    0.03 | 833.3333 |        - |        - |  4127.2 KB |        0.42 |
|                          |       |                      |           |           |           |       |         |          |          |          |            |             |
| GetStringThenDeserialize | 10000 | enumerable-items     | 57.641 ms | 1.9374 ms | 1.2815 ms |  1.00 |    0.00 | 750.0000 | 375.0000 | 125.0000 | 9787.89 KB |        1.00 |
| GetFromJsonAsync         | 10000 | enumerable-items     | 42.829 ms | 1.5815 ms | 0.9411 ms |  0.75 |    0.02 | 833.3333 | 500.0000 | 166.6667 | 4463.65 KB |        0.46 |
| GetFromJsonAsAsyncEnumer | 10000 | enumerable-items     | 41.918 ms | 0.7532 ms | 0.4982 ms |  0.73 |    0.01 | 833.3333 |        - |        - | 4127.37 KB |        0.42 |
|                          |       |                      |           |           |           |       |         |          |          |          |            |             |
| GetStringThenDeserialize | 10000 | list-items           | 54.907 ms | 1.1154 ms | 0.6638 ms |  1.00 |    0.00 | 777.7778 | 444.4444 | 111.1111 | 9795.69 KB |        1.00 |
| GetFromJsonAsync         | 10000 | list-items           | 48.664 ms | 1.2449 ms | 0.8234 ms |  0.89 |    0.02 | 727.2727 | 363.6364 |  90.9091 |  4402.3 KB |        0.45 |
| GetFromJsonAsAsyncEnumer | 10000 | list-items           | 44.453 ms | 0.7312 ms | 0.4836 ms |  0.81 |    0.01 | 818.1818 |        - |        - | 4070.24 KB |        0.42 |

TLDR: When deserializing a list of items choose GetFromJsonAsAsyncEnumerable for the lowest memory footprint and fastest deserialization time.

 */

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 2, iterationCount: 10)]
public class ListDeserializationBenchmark
{
    private HttpClient _httpClient = null!;
    
    [Params( 5, 10000)]
    public int Count = 10;
    
    [Params( "list-items", "enumerable-items", "async-enumerable-items")]
    public string Path = string.Empty;
    
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
    
    private string QueryUri => $"Benchmark/{Path}?count={this.Count}";
    
    [Benchmark(Baseline = true)]
    public async Task GetStringThenDeserialize()
    {
        var stringContent = await this._httpClient.GetStringAsync(QueryUri);
        var result = JsonSerializer.Deserialize<BenchmarkItem[]>(stringContent);
    }
    
    [Benchmark]
    public async Task GetFromJsonAsync()
    {
        var result = await this._httpClient.GetFromJsonAsync<BenchmarkItem[]>(QueryUri);
    }
    
    // Use the lowest memory footprint for list
    [Benchmark]
    public async Task GetFromJsonAsAsyncEnumerable()
    {
        var result = this._httpClient.GetFromJsonAsAsyncEnumerable<BenchmarkItem>(QueryUri);
        await foreach (var item in result)
        {
            // Do nothing
        }
    }
}