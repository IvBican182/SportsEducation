using Aperta_web_app.Data;
using Aperta_web_app.Models.Invitations;

namespace Aperta_web_app.Services.interfaces
{
    public interface IInvitationService
    {
        Task<bool> SendInvitationAsync(UserInvitationDto request);

        Task<UserInvitation> GetUserInvitationByTokenAsync(string token);

        Task MarkAsUsedAsync(int id);
    }
}
