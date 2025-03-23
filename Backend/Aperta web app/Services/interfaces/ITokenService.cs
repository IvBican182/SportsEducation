using Aperta_web_app.Data;
using Microsoft.AspNetCore.Identity;

namespace Aperta_web_app.Services.interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
