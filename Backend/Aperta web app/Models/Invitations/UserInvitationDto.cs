namespace Aperta_web_app.Models.Invitations
{
    public class UserInvitationDto
    {
        public string Email { get; set; }
        public int ClubId { get; set; }
        public int? GroupId { get; set; }
        public string RoleId { get; set; }
    }
}
