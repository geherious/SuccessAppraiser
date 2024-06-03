using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using SuccessAppraiser.BLL.Common.Exceptions;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using System.Security.Claims;

namespace SuccessAppraiser.BLL.Auth.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJwtService _jwtService;

        public TokenService(ApplicationDbContext dbContext, IConfiguration configuration, IJwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }
        public async Task<RefreshToken> AddRefreshTokenAsync(Guid userId, CancellationToken ct = default)
        {
            if (await _dbContext.Users.FindAsync(userId, ct) == null)
            {
                throw new NotFoundException(nameof(userId));
            }

            List<Claim> userClaims =
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            ];

            DateTime expireTime = _jwtService.GetDefaultValidityTime(TokenType.RefreshToken);

            var newToken = new RefreshToken
            {
                Token = _jwtService.GenerateToken(userClaims, TokenType.RefreshToken),
                UserId = userId,
                Expires = expireTime
            };

            await _dbContext.RefreshTokens.AddAsync(newToken, ct);

            await _dbContext.SaveChangesAsync(ct);


            return newToken;

        }

        public async Task RemoveRefreshTokenAsync(string token, CancellationToken ct = default)
        {

            var tokenToDelete = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, ct);

            if (tokenToDelete == null)
            {
                return;
            }

            await RemoveRefreshTokenAsync(tokenToDelete, ct);
        }

        public async Task RemoveRefreshTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            _dbContext.RefreshTokens.Remove(token);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<RefreshToken?> GetValidTokenEntityAsync(string token, CancellationToken ct = default)
        {
            var tokenEntity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, ct);
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
