using SuccessAppraiser.BLL.Auth.Errors;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Enums;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;
using System.Security.Claims;

namespace SuccessAppraiser.BLL.Auth.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IJwtService _jwtService;

        public TokenService(IRefreshTokenRepository refreshTokenRepository, IRepositoryWrapper repositoryWrapper,
            IApplicationUserRepository applicationUserRepository, IJwtService jwtService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _repositoryWrapper = repositoryWrapper;
            _applicationUserRepository = applicationUserRepository;
            _jwtService = jwtService;
        }
        public async Task<RefreshToken> AddRefreshTokenAsync(Guid userId, CancellationToken ct = default)
        {
            if (await _applicationUserRepository.GetByIdAsync(userId, ct) == null)
            {
                throw new InvalidIdException(nameof(ApplicationUser), userId);
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

            await _refreshTokenRepository.AddAsync(newToken, ct);
            await _repositoryWrapper.SaveChangesAsync(ct);

            return newToken;

        }

        public async Task RemoveRefreshTokenAsync(string token, CancellationToken ct = default)
        {
            RefreshToken? tokenToDelete = await _refreshTokenRepository.GetByToken(token, ct);
            if (tokenToDelete == null)
            {
                throw new InvalidTokenException();

            }

            await RemoveRefreshTokenAsync(tokenToDelete, ct);
        }

        public async Task RemoveRefreshTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            _refreshTokenRepository.Delete(token);
            await _repositoryWrapper.SaveChangesAsync(ct);
        }

        public async Task<RefreshToken?> GetValidTokenEntityAsync(string token, CancellationToken ct = default)
        {
            var tokenEntity = await _refreshTokenRepository.GetByToken(token, ct);
            if (tokenEntity == null)
            {
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
