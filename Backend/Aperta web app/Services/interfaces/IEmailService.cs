namespace Aperta_web_app.Services.interfaces
{
    public interface IEmailService 
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
