using Aperta_web_app.Data;
using Aperta_web_app.Models.Authentication;
using Aperta_web_app.Models.Registration;
using Aperta_web_app.Models.User;
using Microsoft.AspNetCore.Identity.Data;
using NuGet.Protocol.Plugins;

namespace Aperta_web_app.Services.interfaces
{
    public interface IAuthService
    {

        Task<(User user, string role)> RegisterUserAsync(string token, UserRegistrationDto request);

        Task<LoginResponseDto> LoginUserAsync(LoginRequestDto request);

        Task<bool> UserExistsAsync(string email);
    }
}
