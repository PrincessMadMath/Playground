#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Text.Json.Serialization;

namespace Benchmark.Json.Models;

public class BenchmarkContent
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("entries")]
    public List<BenchmarkItem> Entries { get; set; }
}

[JsonSerializable(typeof(BenchmarkContent))]
public partial class BenchmarkEntryContentGenerationContext : JsonSerializerContext
{
}