namespace ConfigurationPlayground._2.Binding;

public class BasicOption
{
    internal const string SectionName = "Basic";
    
    public string Name { get; set; }

    public int Count { get; set; }

    public string DefaultValue { get; set; } = "Default value";
}