namespace ConfigurationPlayground._7.OptionsFactory;

public class FactoryOptions
{
    internal const string DefaultSectionName = "FactoryDefault";
    
    internal const string SpecificSectionName = "Factories";
    
    public string Name { get; set; }

    public int WorkHourByWeeks { get; set; }
}