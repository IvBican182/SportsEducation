

namespace Aperta_web_app.Models.Club
{
    public class ClubDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public required string Country { get; set; }

        public required string City { get; set; }

        public required string ContactEmail { get; set; }

        public required string ContactPhone { get; set; }

        public required string Logo { get; set; }  // Path or name of the logo file

        public required string Stadium { get; set; }

        public required bool BillingInfo { get; set; }

        //public List<UserDto> Users { get; set; }
    }
}
