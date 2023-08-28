namespace ConfigurationPlayground._9.DeadlockOfConfiguration;

public class DeadLockOptionsA
{
    public const string SectionName = "DeadA";

    public string DependOnB { get; set; }
}