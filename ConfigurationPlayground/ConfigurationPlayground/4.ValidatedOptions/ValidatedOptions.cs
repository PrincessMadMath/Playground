using System.ComponentModel.DataAnnotations;

namespace ConfigurationPlayground._4.ValidatedOptions;

public class ValidatedOptions
{
    internal const string SectionName = "Validated";
    
    // Don't put [Required] on the properties, because the validation not work properly since not nullable...
    [Range(1, 100, ErrorMessage = "Number must be between 0 and 100")]
    public int NumberA { get; set; }

    [Range(1, 100, ErrorMessage = "Number must be between 0 and 100")]
    public int NumberB { get; set; }

    public int Sum { get; set; }

    [Required]
    [StringLength(1024)]
    public string Checksum { get; set; }
    
    // And more with the Workleap extension library!
}