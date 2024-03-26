using System.Text.Json;

using Benchmark.Json.Models;

using BenchmarkDotNet.Attributes;

namespace Benchmark.Json;

// https://www.stevejgordon.co.uk/using-httpcompletionoption-responseheadersread-to-improve-httpclient-performance-dotnet
/*
| Method              | Count | Path                 | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|-------------------- |------ |--------------------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| ReadCompleteContent | 5     | async(...)items [22] |  1.569 ms | 0.0330 ms | 0.0196 ms |  1.00 |    0.00 |         - |         - |         - |     6.7 KB |        1.00 |
| ReadHeadersOnly     | 5     | async(...)items [22] |  1.594 ms | 0.0760 ms | 0.0502 ms |  1.02 |    0.03 |         - |         - |         - |    5.64 KB |        0.84 |
| WithGetStreamAsync  | 5     | async(...)items [22] |  1.562 ms | 0.1003 ms | 0.0663 ms |  0.99 |    0.03 |         - |         - |         - |    5.61 KB |        0.84 |
|                     |       |                      |           |           |           |       |         |           |           |           |            |             |
| ReadCompleteContent | 5     | enumerable-items     |  1.603 ms | 0.0953 ms | 0.0630 ms |  1.00 |    0.00 |         - |         - |         - |    6.63 KB |        1.00 |
| ReadHeadersOnly     | 5     | enumerable-items     |  1.525 ms | 0.0747 ms | 0.0444 ms |  0.96 |    0.05 |         - |         - |         - |    5.58 KB |        0.84 |
| WithGetStreamAsync  | 5     | enumerable-items     |  1.544 ms | 0.0619 ms | 0.0409 ms |  0.96 |    0.06 |         - |         - |         - |    5.56 KB |        0.84 |
|                     |       |                      |           |           |           |       |         |           |           |           |            |             |
| ReadCompleteContent | 5     | list-items           |  1.530 ms | 0.0412 ms | 0.0245 ms |  1.00 |    0.00 |         - |         - |         - |    6.59 KB |        1.00 |
| ReadHeadersOnly     | 5     | list-items           |  1.553 ms | 0.0907 ms | 0.0600 ms |  1.02 |    0.04 |         - |         - |         - |    5.57 KB |        0.84 |
| WithGetStreamAsync  | 5     | list-items           |  1.536 ms | 0.0926 ms | 0.0612 ms |  1.00 |    0.04 |         - |         - |         - |     5.5 KB |        0.83 |
|                     |       |                      |           |           |           |       |         |           |           |           |            |             |
| ReadCompleteContent | 10000 | async(...)items [22] | 55.271 ms | 1.1416 ms | 0.7551 ms |  1.00 |    0.00 | 1625.0000 | 1625.0000 | 1000.0000 | 8432.91 KB |        1.00 |
| ReadHeadersOnly     | 10000 | async(...)items [22] | 44.947 ms | 1.3677 ms | 0.9046 ms |  0.81 |    0.02 |  777.7778 |  444.4444 |  111.1111 | 4453.54 KB |        0.53 |
| WithGetStreamAsync  | 10000 | async(...)items [22] | 45.168 ms | 1.5722 ms | 1.0399 ms |  0.82 |    0.02 |  727.2727 |  454.5455 |  181.8182 | 4453.25 KB |        0.53 |
|                     |       |                      |           |           |           |       |         |           |           |           |            |             |
| ReadCompleteContent | 10000 | enumerable-items     | 56.391 ms | 1.0368 ms | 0.6858 ms |  1.00 |    0.00 | 1625.0000 | 1625.0000 | 1000.0000 | 8435.46 KB |        1.00 |
| ReadHeadersOnly     | 10000 | enumerable-items     | 44.194 ms | 0.8248 ms | 0.5456 ms |  0.78 |    0.01 |  833.3333 |  500.0000 |  166.6667 | 4454.08 KB |        0.53 |
| WithGetStreamAsync  | 10000 | enumerable-items     | 44.005 ms | 0.6983 ms | 0.4156 ms |  0.78 |    0.01 |  800.0000 |  400.0000 |  100.0000 | 4453.79 KB |        0.53 |
|                     |       |                      |           |           |           |       |         |           |           |           |            |             |
| ReadCompleteContent | 10000 | list-items           | 52.970 ms | 1.8638 ms | 1.2328 ms |  1.00 |    0.00 | 1600.0000 | 1500.0000 | 1000.0000 | 8424.63 KB |        1.00 |
| ReadHeadersOnly     | 10000 | list-items           | 49.471 ms | 1.6139 ms | 1.0675 ms |  0.93 |    0.03 |  727.2727 |  363.6364 |   90.9091 | 4399.49 KB |        0.52 |
| WithGetStreamAsync  | 10000 | list-items           | 48.835 ms | 1.5250 ms | 0.9075 ms |  0.92 |    0.03 |  727.2727 |  363.6364 |   90.9091 | 4399.64 KB |        0.52 |


 */

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 5, iterationCount: 10)]
public class HttpCompletionOptionBenchmark
{
    private HttpClient _httpClient = null!;

    [Params(5, 10000)]
    public int Count = 10;

    [Params("list-items", "enumerable-items", "async-enumerable-items")]
    public string Path = string.Empty;

    private string QueryUri => $"Benchmark/{this.Path}?count={this.Count}";

    [GlobalSetup]
    public void Setup()
    {
        this._httpClient = new HttpClient();
        this._httpClient.BaseAddress = new Uri("https://localhost:7163/");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        this._httpClient.Dispose();
    }

    [Benchmark(Baseline = true)]
    public async Task ReadCompleteContent()
    {
        // Need to insure the dispose of response since is blocking OS resource: socket
        using HttpResponseMessage response =
            await this._httpClient.GetAsync(this.QueryUri, HttpCompletionOption.ResponseContentRead);

        Stream stream = await response.Content.ReadAsStreamAsync();
        BenchmarkItem[]? result = await JsonSerializer.DeserializeAsync<BenchmarkItem[]>(stream);
    }

    [Benchmark]
    public async Task ReadHeadersOnly()
    {
        // Need to insure the dispose of response since is blocking OS resource: socket
        using HttpResponseMessage response =
            await this._httpClient.GetAsync(this.QueryUri, HttpCompletionOption.ResponseHeadersRead);

        Stream stream = await response.Content.ReadAsStreamAsync();
        BenchmarkItem[]? result = await JsonSerializer.DeserializeAsync<BenchmarkItem[]>(stream);
    }

    [Benchmark]
    public async Task WithGetStreamAsync()
    {
        // Need to insure the dispose of response since is blocking OS resource: socket
        await using Stream stream = await this._httpClient.GetStreamAsync(this.QueryUri);

        BenchmarkItem[]? result = await JsonSerializer.DeserializeAsync<BenchmarkItem[]>(stream);
    }
}
