using Aperta_web_app.Services.interfaces;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aperta_web_app.Services.Implementations
{
    public class MailgunEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _domain;
        

        public MailgunEmailService(string apiKey, string domain, HttpClient httpClient)
        {
            this._apiKey = apiKey;
            this._domain = domain;
            this._httpClient = httpClient;

            Console.WriteLine($"Mailgun API Key: {_apiKey}");
            Console.WriteLine($"Mailgun Domain: {_domain}");
        }

        

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://api.mailgun.net/v3/{_domain}/messages"),
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "from", $"Some User Spricko <mailgun@{_domain}>" },
                { "to", to },
                { "subject", subject },
                { "text", body }
            })
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + _apiKey)));

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Test email sent successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to send email. Status Code: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorContent}");
            }
        }
    }
}
