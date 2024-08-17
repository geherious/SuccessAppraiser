using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using SuccessAppraiser.Data.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuccessAppraiser.BLL.Auth.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IEnumerable<Claim> claims, TokenType type)
        {
            DateTime expires = DateTimeFactory(type);

            var issuer = _configuration.GetRequiredSection("JWT:Issuer").Value;
            var audience = _configuration.GetRequiredSection("JWT:Audience").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetRequiredSection("JWT:Key").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            claims = claims.Append(new Claim("RandomFactor", Guid.NewGuid().ToString()));

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

        public DateTime GetDefaultValidityTime(TokenType tokenType)
        {
            return DateTimeFactory(tokenType);
        }

        private DateTime DateTimeFactory(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.AccessToken:
                    int minutes = int.Parse(_configuration.GetRequiredSection("JWT:AccessTokenMinutes").Value!);
                    return DateTime.UtcNow.AddMinutes(minutes);
                case TokenType.RefreshToken:
                    int days = int.Parse(_configuration.GetRequiredSection("JWT:RefreshTokenDays").Value!);
                    return DateTime.UtcNow.AddDays(days);
                default:
                    throw new ArgumentException($"{tokenType} is not supported");
            }
        }

    }
}
