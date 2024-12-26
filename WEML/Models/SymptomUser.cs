using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEML.Areas.Identity.Data;

namespace WEML.Models
{
    public class SymptomUser
    {
        [Key] public Guid SymptomId { get; set; }
        [Key] public string UserId { get; set; }

        [ForeignKey("SymptomId")]
        public virtual Symptom Symptom { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
