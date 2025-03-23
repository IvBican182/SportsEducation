using Microsoft.AspNetCore.Identity;

namespace Aperta_web_app.Data
{
    public class UserInvitation
    {
        public int Id { get; set; }             // Primary key for the Invitation
        public string Email { get; set; }        // Email of the invitee
        // Foreign key to Club
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
        // Navigation property to Club
        // Foreign key to Identity's Role table, but without navigation property
        public string Token { get; set; }        // Token to verify invitation
        public DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; } //checks to see if our user used the token
    }
}
