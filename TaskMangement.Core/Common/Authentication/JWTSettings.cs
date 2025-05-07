namespace TaskManagement.Core.Common.Authentication
{
    /// <summary>
    /// Holds JWT token configuration such as secret key, issuer, audience, and expiration settings.
    /// Populated via appsettings.json.
    /// </summary>
    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
