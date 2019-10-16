namespace Adsboard.Api.Authentication
{
    public class AuthenticationOptions
    {
        public string Provider { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public int ExpiryMinutes { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateAudience { get; set; }   
        public string ValidAudience { get; set; }
    }
}