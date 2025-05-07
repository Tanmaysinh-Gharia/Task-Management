using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagement.Core.Enums;

namespace TaskManagement.Core.Helpers
{
    public class JWTGenerator
    {

        private readonly IConfiguration _config;

        public JWTGenerator(IConfiguration config)
        {
            _config = config;
        }
        
        /// <summary>
        /// Generates a JWT access token containing user ID, email, role, and IP address claims.
        /// Token expiration and signing credentials are derived from configuration.
        /// </summary>
        public string GenerateAccessToken(string email, string id, string ipAddress, Role userRole)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role , userRole.ToString()),
                new Claim("ipAddress", ipAddress)
            };

            SigningCredentials creads = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256Signature
            );

            double accessTokenExpirationMinutes = double.Parse(_config["Jwt:AccessTokenExpirationMinutes"]);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(accessTokenExpirationMinutes),
                signingCredentials: creads
            );
            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return serializedToken;
        }

        /// <summary>
        /// Generates a secure, random 64-byte refresh token encoded in Base64 format.
        /// </summary>
        public string GenerateRefreshToken()
        {
            // Generate a random 64-byte string and convert it to Base64
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

    }
}
