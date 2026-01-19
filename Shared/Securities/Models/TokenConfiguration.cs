namespace Shared.Securities.Models
{
    public class TokenConfiguration
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string SecretKey { get; set; }
        public required string ExpireDay { get; set; }
        public required string TokenCookieName { get; set; }
    }
}
