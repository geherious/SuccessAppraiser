using Microsoft.IdentityModel.Tokens;
using SuccessAppraiser.Entities;
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
        public string GetAccessToken(ApplicationUser user, IList<Claim> claims)
        {

            List<Claim> tokenClaims = new List<Claim>();
            tokenClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            tokenClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            tokenClaims.Concat(claims);


            var issuer = _configuration.GetSection("JWT:Issuer").Value;
            var audience = _configuration.GetSection("JWT:Audience").Value;
            var expiresInMinutes = int.Parse(_configuration.GetSection("JWT:ExpiresInMinutes").Value);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: tokenClaims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
    