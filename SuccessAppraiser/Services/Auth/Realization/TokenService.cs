using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Auth.Errors;
using SuccessAppraiser.Services.Auth.Interfaces;

namespace SuccessAppraiser.Services.Auth.Realization
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public TokenService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<Guid> AddRefreshTokenAsync(Guid userId)
        {
            RemoveAllExpiredRefreshTokensQuery(userId);

            int expriresInDays = int.Parse(_configuration.GetSection("JWT:RefreshTokenDays").Value);

            if (await _dbContext.Users.FindAsync(userId) == null)
            {
                throw new UserNotFoundException();
            }

            var newToken = new RefreshToken
            {
                Token = Guid.NewGuid(),
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(expriresInDays)
            };

            await _dbContext.RefreshTokens.AddAsync(newToken);

            await _dbContext.SaveChangesAsync();

            return newToken.Token;

        }

        public void RemoveAllExpiredRefreshTokens(Guid userId)
        {
            RemoveAllExpiredRefreshTokensQuery(userId);

            _dbContext.SaveChanges();
        }

        private void RemoveAllExpiredRefreshTokensQuery(Guid userId)
        {
            _dbContext.RefreshTokens
                .RemoveRange(_dbContext.RefreshTokens.Where(x => (x.UserId == userId) && (x.Expires > DateTime.UtcNow)));
        }

        public async Task RemoveRefreshTokenAsync(Guid userId, Guid token)
        {
            RemoveAllExpiredRefreshTokensQuery(userId);

            if (await _dbContext.Users.FindAsync(userId) == null)
            {
                throw new UserNotFoundException();
            }

            var tokenToDelete = await _dbContext.RefreshTokens.Where(x => (x.UserId == userId) && (x.Token == token)).FirstOrDefaultAsync();

            if (tokenToDelete == null)
            {
                return;
            }

            _dbContext.RefreshTokens.Remove(tokenToDelete);

            await _dbContext.SaveChangesAsync();
        }
    }
}
