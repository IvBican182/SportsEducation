using Aperta_web_app.Data;
using Aperta_web_app.Models.Authentication;
using Aperta_web_app.Models.Registration;
using Aperta_web_app.Models.User;
using Aperta_web_app.Services.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace Aperta_web_app.Services.Implementations
{
    public class authService : IAuthService
    {
        private readonly IInvitationService _invitationService;
        private readonly UserManager<User> _userManager; // Identity UserManager for creating users and handling passwords
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public authService(AppDbContext dbContext, IInvitationService invitationService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _invitationService = invitationService;
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _tokenService = tokenService;

        }


        public async Task<(User user, string role)> RegisterUserAsync(string token, UserRegistrationDto data)
        {

            var invitation = await _invitationService.GetUserInvitationByTokenAsync(token);
            var newUser = new User
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                ParentFirstName = data.ParentFirstName,
                ParentLastName = data.ParentLastName,
                BirthDate = data.BirthDate,
                ClubId = invitation.ClubId,
                GroupId = invitation.GroupId,
                Email = invitation.Email,
                UserName = invitation.Email
            };

            var result = await _userManager.CreateAsync(newUser, data.Password);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            var roleData = await _roleManager.FindByIdAsync(invitation.RoleId.ToString());
            string role = null;
            if (roleData != null)
            {
                await _userManager.AddToRoleAsync(newUser, roleData.Name);
                role = roleData.Name;
            }

            invitation.IsUsed = true;
            await _invitationService.MarkAsUsedAsync(invitation.Id);

            return (newUser, role);
           // return Ok(new { Token = token, Role = role, User = user });


        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto data) 
        {
            // Step 1: Find the user by email
            var user = await _userManager.FindByEmailAsync(data.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password!"); // User not found
            }

            // Step 2: Verify the password
            var validPassword = await _userManager.CheckPasswordAsync(user, data.Password);

            if (!validPassword)
            {
                throw new Exception("Invalid email or password!"); // Invalid password
            }

            // Step 3: Generate a JWT Token
            var token = await _tokenService.GenerateTokenAsync(user); // Assuming _tokenService.GenerateToken() returns a string

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Failed to generate token."); // Token generation failed
            }

            // Step 4: Return the LoginResponseDto with token and user details
            var response = new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return response; // Return 

        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
