using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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


        public string GenerateRefreshToken()
        {
            // Generate a random 64-byte string and convert it to Base64
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

    }
}
