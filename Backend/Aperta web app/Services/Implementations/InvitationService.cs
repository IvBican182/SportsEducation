using Aperta_web_app.Data;
using Aperta_web_app.Models.Invitations;
using Aperta_web_app.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aperta_web_app.Services.Implementations
{
    public class InvitationService : IInvitationService
    {
        private readonly IEmailService _emailService;
        private readonly AppDbContext _dbContext;

        public InvitationService(IEmailService emailService, AppDbContext dbContext)
        {
            _emailService = emailService;
            _dbContext = dbContext;
            
        }

        public async Task<bool> SendInvitationAsync(UserInvitationDto request)
        {
            // Generate token (you can use a GUID or JWT)
            var token = Guid.NewGuid().ToString();

            // Create invitation record
            var invitation = new UserInvitation
            {
                Email = request.Email,
                ClubId = request.ClubId,
                GroupId = request.GroupId,
                RoleId = request.RoleId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                IsUsed = false

            };

            _dbContext.UserInvitations.Add(invitation);
            await _dbContext.SaveChangesAsync();

            // Send invitation email with token
            var invitationUrl = $"http://localhost:5173/register?token={token}";
            await _emailService.SendEmailAsync(request.Email, "You're invited to join as General Admin", $"Click here to register: {invitationUrl}");

            return true;
        }

        public async Task<UserInvitation> GetUserInvitationByTokenAsync(string token)
        {
            var userInvitation = await _dbContext.UserInvitations.FirstOrDefaultAsync(x => x.Token == token);

            if (userInvitation == null)
            {
                return null;
            }

            return userInvitation;
        }

        public async Task MarkAsUsedAsync(int id)
        {
            var invitation = await _dbContext.UserInvitations.FirstOrDefaultAsync(q => q.Id == id);

            if (invitation == null)
            {
                throw new InvalidOperationException("Invitation not found.");
            }

            invitation.IsUsed = true;  // Mark the invitation as used

            _dbContext.UserInvitations.Update(invitation);
            await _dbContext.SaveChangesAsync();

            _dbContext.UserInvitations.Remove(invitation);
            await _dbContext.SaveChangesAsync();

        }
    }
}
