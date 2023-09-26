using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._4.ValidatedOptions;

public class OptionValidator: IValidateOptions<ValidatedOptions>
{
    private readonly InjectedValidator _checksumValidator;

    public OptionValidator(InjectedValidator checksumValidator)
    {
        _checksumValidator = checksumValidator;
    }
    
    // Warning: it's does not return a Task!
    public ValidateOptionsResult Validate(string? name, ValidatedOptions options)
    {
        if (!this._checksumValidator.IsChecksumValid(options.Checksum))
        {
            // Could support passing a list of errors if doing multiple checks        
            return ValidateOptionsResult.Fail("Invalid checksum");
        }
        
        return ValidateOptionsResult.Success;
    }
}