using Aperta_web_app.Models.Invitations;
using Aperta_web_app.Services;
using Aperta_web_app.Services.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aperta_web_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendInvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public SendInvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost("send-invite")]
        public async Task<IActionResult> SendInvite([FromBody] UserInvitationDto request)
        {
            var result = await _invitationService.SendInvitationAsync(request);

            if (result)
                return Ok("Invitation sent successfully");

            return BadRequest("Failed to send invitation");

        }

    }
}
