using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEML.Models;

namespace WEML.Areas.Identity.Data
{
    public class User : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public Guid UserId {  get; set; }
        [Required]
        [PersonalData]
        [Column(TypeName ="nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string DateOfBirth { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string numberOfPoints { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string currentDiagnosis { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string ContactPersonPhone { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string ContactDoctorEmail { get; set; }

        public virtual ICollection<SymptomUser> SymptomUsers { get; set; }
        public virtual ICollection<FeelingUser> FeelingUsers { get; set; }
    }
}
