using System.ComponentModel.DataAnnotations;

namespace Aperta_web_app.Models.Registration
{
    public class UserRegistrationDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? ParentFirstName { get; set; }

        [MaxLength(50)]
        public string? ParentLastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
