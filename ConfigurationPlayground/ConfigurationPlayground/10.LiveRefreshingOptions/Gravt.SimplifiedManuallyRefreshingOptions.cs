using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;

namespace ConfigurationPlayground._10.LiveRefreshingOptions;

public sealed class SimplifiedAppConfigurationRefresherService : BackgroundService
{
    private readonly IConfigurationRefresherProvider _configurationRefresherProvider;

    public SimplifiedAppConfigurationRefresherService(IConfigurationRefresherProvider configurationRefresherProvider)
    {
        this._configurationRefresherProvider = configurationRefresherProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var refresher in this._configurationRefresherProvider.Refreshers)
            {
                refresher.TryRefreshAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}