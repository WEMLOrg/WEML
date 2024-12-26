using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEML.Areas.Identity.Data;

namespace WEML.Models
{
    public class FeelingUser
    {
        public Guid FeelingId { get; set; }
        public string UserId { get; set; } 

        [ForeignKey("FeelingId")]
        public virtual Feeling Feeling { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
