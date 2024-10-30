using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEML.Models
{
    public class User : IdentityUser
    {
        [Key]public Guid UserId {  get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int numberOfPoints { get; set; }
        public string currentDiagnosis { get; set; }
    }
}
