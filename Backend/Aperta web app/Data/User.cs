using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace Aperta_web_app.Data
{
    public class User : IdentityUser
    {

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        [MaxLength(50)]
        public string? ParentFirstName { get; set; }

        [MaxLength(50)]
        public string? ParentLastName { get; set; }

        public bool? BillingDetails { get; set; }  // true or false

        // foreign key relationships
        [ForeignKey("Club")]
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }

        [ForeignKey("Group")]
        public int? GroupId { get; set; }
        public virtual Group? Group { get; set; }

      //  public override string UserName { get; set; } = string.Empty;
       // public override string NormalizedUserName { get; set; } = string.Empty;

        //[ForeignKey("Role")]
        //public int RoleId { get; set; }  // Foreign key for Role
        //public virtual Role Role { get; set; }
    }
}
