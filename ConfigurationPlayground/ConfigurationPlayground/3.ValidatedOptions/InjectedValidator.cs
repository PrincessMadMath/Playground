namespace ConfigurationPlayground._4.ValidatedOptions;

public class InjectedValidator
{
    public bool IsChecksumValid(string checksum)
    {
        return checksum == "123";
    }
}