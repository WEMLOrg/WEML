using System.ComponentModel.DataAnnotations;

namespace WEML.Models;

public class Feeling
{
    [Key] public Guid FeelingId { get; set; } 
    public String FeelingName { get; set; }
    public String FeelingDescription { get; set; }
    public String FeelingSeverity { get; set; }
    public DateTime DateTime { get; set; }

    public virtual ICollection<FeelingUser> FeelingUser { get; set; }
}