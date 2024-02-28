using SuccessAppraiser.Entities;
using System.Security.Claims;

namespace SuccessAppraiser.Services.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(IList<Claim> claims, TokenType tokenType);
    }
}
