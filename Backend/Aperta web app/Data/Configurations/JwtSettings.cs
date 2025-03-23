namespace Aperta_web_app.Data.Configurations
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenExpirationInHours { get; set; }
    }
}
