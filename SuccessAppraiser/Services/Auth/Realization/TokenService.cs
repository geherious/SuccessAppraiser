using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Auth.Errors;
using SuccessAppraiser.Services.Auth.Interfaces;
using System.Security.Claims;

namespace SuccessAppraiser.Services.Auth.Realization
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public TokenService(ApplicationDbContext dbContext, IConfiguration configuration, IJwtService jwtService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _jwtService = jwtService;
        }
        public async Task<RefreshToken> AddRefreshTokenAsync(Guid userId)
        {

            int expriresInDays = int.Parse(_configuration.GetSection("JWT:RefreshTokenDays").Value);

            if (await _dbContext.Users.FindAsync(userId) == null)
            {
                throw new UserNotFoundException();
            }

            List<Claim> userClaims =
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            ];

            var newToken = new RefreshToken
            {
                Token = _jwtService.GenerateToken(userClaims, TokenType.RefreshToken),
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(expriresInDays)
            };

            await _dbContext.RefreshTokens.AddAsync(newToken);

            await _dbContext.SaveChangesAsync();
            

            return newToken;

        }

        public async Task RemoveRefreshTokenAsync(string token)
        {

            var tokenToDelete = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

            if (tokenToDelete == null)
            {
                return;
            }

            _dbContext.RefreshTokens.Remove(tokenToDelete);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRefreshTokenAsync(RefreshToken token)
        {
            _dbContext.RefreshTokens.Remove(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetValidTokenEntityAsync(string token)
        {
            var tokenEntity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            if (tokenEntity == null)
            {
                // TODO: need to remove all tokens
                return null;
            }

            if (tokenEntity.Expires <= DateTime.UtcNow)
            {
                return null;
            }
            else
            {
                return tokenEntity;
            }
        }
    }
}
