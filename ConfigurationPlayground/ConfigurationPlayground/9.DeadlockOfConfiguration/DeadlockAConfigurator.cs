using ConfigurationPlayground._1.Configure;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._9.DeadlockOfConfiguration;

public class DeadlockAConfigurator: IConfigureOptions<DeadLockOptionsA>
{
    private readonly IOptionsMonitor<DeadLockOptionsB> _optionsB;
    private readonly ConfigureServices _configureServices;

    public DeadlockAConfigurator(IOptionsMonitor<DeadLockOptionsB> optionsB)
    {
        _optionsB = optionsB;
    }
    
    public void Configure(DeadLockOptionsA options)
    {
        options.DependOnB = this._optionsB.CurrentValue.DependOnA;
    }
}