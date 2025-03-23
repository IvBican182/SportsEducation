using System.ComponentModel.DataAnnotations;

namespace Aperta_web_app.Models.Club
{
    //base dto that will provide inheritance to other DTOs to avoid repetition
    public abstract class BaseClubDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Country { get; set; }

        [Required]
        [MaxLength(100)]
        public required string City { get; set; }

        [Required]
        [EmailAddress]
        public required string ContactEmail { get; set; }

        [Required]
        [MaxLength(15)]
        public required string ContactPhone { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Logo { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Stadium { get; set; }

        [Required]
        public required bool BillingInfo { get; set; }

        public string StripeId { get; set; }

        
    }

}

