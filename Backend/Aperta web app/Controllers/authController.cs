using Aperta_web_app.Models.Registration;
using Aperta_web_app.Services.Implementations;
using Aperta_web_app.Services.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Aperta_web_app.Data;
using NuGet.Common;
using System.Data;

namespace Aperta_web_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IInvitationService _invitationService;
        private readonly IAuthService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public authController(IInvitationService invitationService, IAuthService userService, UserManager<User> userManager, ITokenService tokenService)
        {
            _invitationService = invitationService;
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;

        }

        [HttpPost("auth/registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto request)
        {
            // Step 1: Validate the invitation token
            var invitation = await _invitationService.GetUserInvitationByTokenAsync(request.Token);
            if (invitation == null || invitation.IsUsed)
            {
                return BadRequest(new 
                { 
                    Message = "Invalid or expired invitation token.",
                    ErrorCode = "InvalidToken"
                });
            }

            if (!string.Equals(invitation.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    Message = "The email does not match the invitation.",
                    ErrorCode = "EmailMismatch"
                });
            }

            // Step 2: Check if the email already exists
            bool userExists = await _userService.UserExistsAsync(invitation.Email);
            if (userExists)
            {
                return BadRequest(new { Message = "Email is already registered." });
            }

            // Step 3: Proceed with user registration
            try
            {
                var (user, role) = await _userService.RegisterUserAsync(request.Token, request);
                var token = await _tokenService.GenerateTokenAsync(user);
                return Ok(new { user, role, token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Failed to register user: {ex.Message}" });
            }

            
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Unauthorized(
                    new 
                    { 
                        Message = "Invalid email or password.",
                        ErrorCode = "InvalidEmailorPassword"
                    });
            }

            // Retrieve the roles for the user from the AspNetUserRoles table
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var token = await _tokenService.GenerateTokenAsync(user);

            return Ok(new { Token = token, Role = role, User = user });

        
        }
    }
}
