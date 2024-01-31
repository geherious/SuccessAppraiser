using SuccessAppraiser.Entities;
using System.Security.Claims;

namespace SuccessAppraiser.Services.Auth.Interfaces
{
    public interface IJwtService
    {
        string GetAccessToken(ApplicationUser user, IList<Claim> claims);
    }
}
