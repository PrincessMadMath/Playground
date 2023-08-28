using ConfigurationPlayground._1.Configure;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._9.DeadlockOfConfiguration;

public class DeadlockBConfigurator: IConfigureOptions<DeadLockOptionsB>
{
    private readonly IOptionsMonitor<DeadLockOptionsA> _optionsA;
    private readonly ConfigureServices _configureServices;

    public DeadlockBConfigurator(IOptionsMonitor<DeadLockOptionsA> optionsA)
    {
        _optionsA = optionsA;
    }
    
    public void Configure(DeadLockOptionsB options)
    {
        options.DependOnA = this._optionsA.CurrentValue.DependOnB;
    }
}