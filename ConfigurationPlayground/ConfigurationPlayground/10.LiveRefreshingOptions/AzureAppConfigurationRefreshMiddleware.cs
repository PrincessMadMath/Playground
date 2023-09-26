using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace ConfigurationPlayground._10.LiveRefreshingOptions;

public static class AzureAppConfigurationExtensions
{
    /// <summary>
    /// Configures a middleware for Azure App Configuration to use activity-based refresh for data configured in the provider.
    /// </summary>
    /// <param name="builder">An instance of <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /></param>
    public static IApplicationBuilder UseAzureAppConfiguration(this IApplicationBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));
        return builder.ApplicationServices.GetService(typeof(IConfigurationRefresherProvider)) != null ? builder.UseMiddleware<AzureAppConfigurationRefreshMiddleware>() : throw new InvalidOperationException("Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddAzureAppConfiguration' inside the call to 'ConfigureServices(...)' in the application startup code.");
    }
}

/// <summary>
/// Middleware for Azure App Configuration to use activity-based refresh for key-values registered in the provider.
/// </summary>
internal class AzureAppConfigurationRefreshMiddleware
{
    private readonly RequestDelegate _next;

    public IEnumerable<IConfigurationRefresher> Refreshers { get; }

    public AzureAppConfigurationRefreshMiddleware(RequestDelegate next, IConfigurationRefresherProvider refresherProvider)
    {
        this._next = next;
        this.Refreshers = refresherProvider.Refreshers;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        foreach (IConfigurationRefresher refresher in this.Refreshers)
            refresher.TryRefreshAsync();
        await this._next(context).ConfigureAwait(false);
    }
}













// Disregard everything below here, it's just there to make the code compile
public delegate Task RequestDelegate(HttpContext context);

public class HttpContext
{
}