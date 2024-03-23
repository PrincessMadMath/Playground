using System.Text.Json.Serialization;

namespace Benchmark.Json.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class BenchmarkItem
{
    [JsonPropertyName("API")] 
    public string Api { get; set; }

    [JsonPropertyName("Description")] 
    public string Description { get; set; }

    [JsonPropertyName("Auth")] 
    public string Auth { get; set; }

    [JsonPropertyName("HTTPS")] 
    public bool Https { get; set; }

    [JsonPropertyName("Cors")] 
    public string Cors { get; set; }

    [JsonPropertyName("Category")] 
    public string Category { get; set; }

    [JsonPropertyName("Links")] 
    public string Link { get; set; }
}

[JsonSerializable(typeof(BenchmarkItem))]
public partial class BenchmarkItemContentGenerationContext : JsonSerializerContext
{
}