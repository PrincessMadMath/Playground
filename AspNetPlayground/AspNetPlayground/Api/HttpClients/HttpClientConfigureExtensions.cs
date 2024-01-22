using Polly;
using Polly.Extensions.Http;

namespace Api.HttpClients;

public static class HttpClientConfigureExtensions
{
    // https://www.milanjovanovic.tech/blog/the-right-way-to-use-httpclient-in-dotnet
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        // TODO: Add default HttpClient
        services.AddHttpClient("")
            .AddPolicyHandler(GetRetryPolicy());
        
        
        return services;
    }
    
    // https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}