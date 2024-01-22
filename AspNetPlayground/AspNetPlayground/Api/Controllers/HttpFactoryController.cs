using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class HttpFactoryController: ControllerBase
{
    private readonly HttpClient _httpClient;

    public HttpFactoryController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    [HttpGet]
    [Route("classic")]
    public async Task<IActionResult> ClassicDeserialisationo()
    {
        var stopwatch = Stopwatch.StartNew();   
        
        // Will load the entire response into memory before deserialising
        var content = await this._httpClient.GetFromJsonAsync<EntryContent>("https://api.publicapis.org/entries");
        
        return this.Ok(content.Entries);
    }
    
    // https://www.stevejgordon.co.uk/using-httpcompletionoption-responseheadersread-to-improve-httpclient-performance-dotnet
    [HttpGet]
    [Route("headers-only")]
    public async Task<IActionResult> HeadersOnly()
    {
        var stopwatch = Stopwatch.StartNew();   

        // Need to insure the dispose of response since is blocking OS resource: socket
        using var response = await this._httpClient.GetAsync("https://api.publicapis.org/entries", HttpCompletionOption.ResponseHeadersRead);
        
        response.EnsureSuccessStatusCode();
        
        if (response.Content is object)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<EntryContent>(stream);

            return this.Ok(data.Entries);
        }

        return this.BadRequest();
    }
    
    
    [HttpGet]
    [Route("async-enumerable")]
    public async Task<IActionResult> FromJsonAsyncEnumerable()
    {
        // Seems to only wait for the header before streaming: sweet!
        // Yeah this format prevent it...
        var content = await this._httpClient.GetFromJsonAsAsyncEnumerable<Entry>("https://api.publicapis.org/entries").ToListAsync();
        
        return this.Ok(content);
    }
    
    [HttpGet]
    [Route("fancy-async-enumerable")]
    public async IAsyncEnumerable<Entry> FancyAsyncEnumerable()
    {
        // How to be able to return async enumerable of the .Entry without loading all the data in memory?
        // Maybe this: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-utf8jsonreader
        
        yield break;
    }
    
    public class EntryContent
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        
        [JsonPropertyName("entries")]
        public List<Entry> Entries { get; set; }
    }


    public class Entry
    {
        [JsonPropertyName("API")]
        public string Api { get; set; }

        [JsonPropertyName("Ddescription")]
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
}