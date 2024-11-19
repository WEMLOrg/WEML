using System.ComponentModel.DataAnnotations;

namespace WEML.Models;

public class Symptom
{
    [Key] public Guid SymptomId { get; set; } 
    public String SymptomName { get; set; }
    public String SymptomDescription { get; set; }
    public String Severity { get; set; }
    public DateTime DateTime { get; set; }
}