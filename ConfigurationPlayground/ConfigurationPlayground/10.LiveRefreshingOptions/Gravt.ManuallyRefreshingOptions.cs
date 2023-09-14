using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._10.LiveRefreshingOptions;

public sealed class AppConfigurationRefresherService : BackgroundService
{
    private readonly IConfigurationRefresherProvider _configurationRefresherProvider;
    private readonly ILogger<AppConfigurationRefresherService> _logger;
    private readonly IOptionsMonitor<RefresherOptions> _optionsMonitor;

    public AppConfigurationRefresherService(IConfigurationRefresherProvider configurationRefresherProvider, ILogger<AppConfigurationRefresherService> logger, IOptionsMonitor<RefresherOptions> optionsMonitor)
    {
        this._configurationRefresherProvider = configurationRefresherProvider;
        this._logger = logger;
        this._optionsMonitor = optionsMonitor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(this._optionsMonitor.CurrentValue.RefreshRate, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var refresher in this._configurationRefresherProvider.Refreshers)
                {
                    if (!await refresher.TryRefreshAsync(stoppingToken))
                    {
                        this._logger.LogWarning("Fail to refresh the configs");
                    }
                }

                await Task.Delay(this._optionsMonitor.CurrentValue.RefreshRate, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            this._logger.LogInformation("{Service} stopped", stoppingToken);
        }
        catch (Exception ex)
        {
            this._logger.LogCritical(ex, "{Service} crashed", nameof(AppConfigurationRefresherService));

            throw;
        }
    }
}







// Disregard everything below here, it's just there to make the code compile
public sealed class RefresherOptions
{
    /// <remarks>
    /// https://docs.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-aspnet-core?tabs=core5x#add-a-sentinel-key
    /// </remarks>>
    public string SentinelKey { get; set; } = null!;

    public TimeSpan RefreshRate { get; set; } = TimeSpan.FromMinutes(1);
}