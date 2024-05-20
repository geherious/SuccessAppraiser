using Data.Enums;
using System.Security.Claims;

namespace SuccessAppraiser.BLL.Auth.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(IEnumerable<Claim> claims, TokenType tokenType);
        DateTime GetDefaultValidityTime(TokenType tokenType);
    }
}
