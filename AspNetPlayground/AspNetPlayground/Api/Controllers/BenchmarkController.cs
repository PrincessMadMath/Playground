using Benchmark.Json.Models;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BenchmarkController: ControllerBase
{
    private readonly Faker<BenchmarkItem> _faker = new Faker<BenchmarkItem>()
        .RuleFor(x => x.Api, f => f.Internet.Url())
        .RuleFor(x => x.Auth, f => f.Lorem.Word())
        .RuleFor(x => x.Category, f => f.Lorem.Word())
        .RuleFor(x => x.Cors, f => f.Lorem.Word())
        .RuleFor(x => x.Description, f => f.Lorem.Sentence())
        .RuleFor(x => x.Https, f => f.Random.Bool())
        .RuleFor(x => x.Link, f => f.Internet.Url());
    
    [HttpGet]
    [Route("list-items")]
    public IList<BenchmarkItem> GetListItems([FromQuery] int count)
    {
        return _faker.Generate(count);
    }
    
    [HttpGet]
    [Route("enumerable-items")]
    public IEnumerable<BenchmarkItem> GetEnumerableItems([FromQuery] int count)
    {
        for(var i = 0; i < count; i++)
        {
            yield return _faker.Generate();
        }
    }
    
    [HttpGet]
    [Route("async-enumerable-items")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async IAsyncEnumerable<BenchmarkItem> GeAsyncEnumerableItems(int count)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        for(var i = 0; i < count; i++)
        {
            yield return _faker.Generate();
        }
    }
    
    [HttpGet]
    [Route("content")]
    public BenchmarkContent GetContent([FromQuery] int count)
    {
        return new BenchmarkContent
        {
            Count = count,
            Entries = _faker.Generate(count)
        };
    }
}