using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._6.NamedOptionsRefreshBug;

public class ProblemNamedOptionValidator: IValidateOptions<ProblemNamedOptions>
{
    // Problem #2: Get call with DefaultNamed options which doesn't exist
    public ValidateOptionsResult Validate(string? name, ProblemNamedOptions options)
    {
        if (string.IsNullOrEmpty(options.Value))
        {
            return ValidateOptionsResult.Fail("Value is empty");
        }
        
        return ValidateOptionsResult.Success;
    }
}