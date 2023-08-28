using Microsoft.Extensions.Configuration;

namespace ConfigurationPlayground._0.ConfigurationSource;

// 1. Inherit from ConfigurationProvider
public class CustomConfigurationProvider: ConfigurationProvider
{
    public static int Counter = 0;
    private readonly Timer _timer;
    
    public CustomConfigurationProvider()
    {
        _timer = new Timer(StartMonitor, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
    }
    
    public override void Load()
    {
        // 2. Put data in this.Data
        this.Data = new Dictionary<string, string?>
        {
            { "Source:CustomKey", Counter.ToString() }
        };
    }

    private async void StartMonitor(object? state)
    {
        // 3. Watch change and trigger reload
        Counter++;
        this.Load();
        this.OnReload();
    }
}