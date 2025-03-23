namespace Aperta_web_app.Models.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ClubId { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
