using Microsoft.Extensions.Http;

using Polly;
using Polly.Extensions.Http;

namespace Api.HttpClients;

public static class HttpClientConfigureExtensions
{
    public const string Custom = "Custom";

    // https://www.milanjovanovic.tech/blog/the-right-way-to-use-httpclient-in-dotnet
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient(string.Empty)
            .AddPolicyHandler(GetRetryPolicy()); // Add a HttpMessageHandler using Polly policy

        ConfigureCustomHttpClient(services);


        return services;
    }

    private static void ConfigureCustomHttpClient(IServiceCollection services)
    {
        services.AddTransient<RandomHeaderHandler>();
        services.AddHttpClient(Custom)
            .ConfigureHttpClient((provider, client) =>
            {
                // Could use provider to get IOptions
                client.BaseAddress = new Uri("https://api.publicapis.org/");
            })
            .ConfigureAdditionalHttpMessageHandlers((list, provider) =>
            {
                // Add HttpClient Message Handler (https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/httpclient-message-handlers)
                // Could be useful to fetch token and add in the header 
                var randomHeaderHandler = provider.GetRequiredService<RandomHeaderHandler>();
                list.Add(randomHeaderHandler);
            })
            .AddStandardResilienceHandler() // From Microsoft.Extensions.Http.Resilience
            ;

        // Can later configure the HttpClientFactoryOptions for the same name
        services.Configure<HttpClientFactoryOptions>(Custom, options =>
        {
            options.ShouldRedactHeaderValue = s =>
            {
                if (string.Equals(s, "Authorization", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            };
        });
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
