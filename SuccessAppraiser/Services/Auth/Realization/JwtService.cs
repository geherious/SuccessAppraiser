
using Microsoft.IdentityModel.Tokens;
using SuccessAppraiser.Services.Auth.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuccessAppraiser.Services.Auth.Realization
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IList<Claim> claims, TokenType type)
        {
            DateTime expires = DateTimeFactory(type);

            var issuer = _configuration.GetSection("JWT:Issuer").Value;
            var audience = _configuration.GetSection("JWT:Audience").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private DateTime DateTimeFactory(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Accesstoken:
                    int minutes = int.Parse(_configuration.GetSection("JWT:AccessTokenMinutes").Value);
                    return DateTime.UtcNow.AddMinutes(minutes);
                case TokenType.RefreshToken:
                    int days = int.Parse(_configuration.GetSection("JWT:RefreshTokenDays").Value);
                    return DateTime.UtcNow.AddDays(days);
                default:
                    return DateTime.UtcNow;
            }
        }

    }
}
    