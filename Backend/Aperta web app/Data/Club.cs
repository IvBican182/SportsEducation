using System.ComponentModel.DataAnnotations;

namespace Aperta_web_app.Data
{
    public class Club
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Country { get; set; }

        public required string City { get; set; }

        public required string ContactEmail { get; set; }

        public required string ContactPhone { get; set; }

        public required string Logo { get; set; }  // Path or name of the logo file

        public required string Stadium { get; set; }

        public required bool BillingInfo { get; set; }  // true or false

        public string? StripeId { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Group> Groups { get; set; }



        // Navigation property to link with Users if needed
        //public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}