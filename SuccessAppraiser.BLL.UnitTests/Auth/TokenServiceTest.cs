using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SuccessAppraiser.BLL.Auth.Errors;
using SuccessAppraiser.BLL.Auth.Services;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.UnitTests.TestObjects;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Enums;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;
using System.Security.Claims;

namespace SuccessAppraiser.BLL.UnitTests.Auth
{
    public class TokenServiceTest
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        private readonly IApplicationUserRepository _applicationUserRepository = Substitute.For<IApplicationUserRepository>();
        private readonly IRepositoryWrapper _repositoryWrapper = Substitute.For<IRepositoryWrapper>();
        private readonly IJwtService _jwtService = Substitute.For<IJwtService>();
        private readonly TokenService _service;

        public TokenServiceTest()
        {
            _service = new TokenService(_refreshTokenRepository, _repositoryWrapper,
                _applicationUserRepository, _jwtService);
        }

        [Fact]
        public async Task AddRefreshTokenAsync_ShouldReturnNewToken()
        {
            ApplicationUser user = AuthTestObjects.GetApplicationUser();
            string tokenString = Guid.NewGuid().ToString();
            DateTime expires = DateTime.Now.AddDays(2);
            _applicationUserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);
            _jwtService.GetDefaultValidityTime(TokenType.RefreshToken).Returns(expires);
            _jwtService.GenerateToken(Arg.Any<List<Claim>>(), TokenType.RefreshToken).Returns(tokenString);

            RefreshToken newToken = await _service.AddRefreshTokenAsync(user.Id);

            newToken.Token.Should().NotBeNull();
            newToken.Token.Should().Be(tokenString);
            newToken.Expires.Should().Be(expires);
            newToken.UserId.Should().Be(user.Id);

            await _refreshTokenRepository.Received(1)
                .AddAsync(Arg.Any<RefreshToken>(), Arg.Any<CancellationToken>());

            await _repositoryWrapper.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task AddRefreshTokenAsync_ShouldThrow_WhenUserDoesNotExist()
        {
            _applicationUserRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

            Func<Task> act = () => _service.AddRefreshTokenAsync(Guid.NewGuid());

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(ApplicationUser));

        }

        [Fact]
        public async Task RemoveRefreshTokenAsync_ShouldRemove()
        {
            RefreshToken refreshToken = AuthTestObjects.GetRefreshToken();
            _refreshTokenRepository.GetByToken(refreshToken.Token, Arg.Any<CancellationToken>()).Returns(refreshToken);

            await _service.RemoveRefreshTokenAsync(refreshToken.Token);

            _refreshTokenRepository.Received(1)
                .Delete(refreshToken);

            await _repositoryWrapper.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RemoveRefreshTokenAsync_ShouldThrow_WhenTokenDoesNotExist()
        {
            RefreshToken refreshToken = AuthTestObjects.GetRefreshToken();
            _refreshTokenRepository.GetByToken(refreshToken.Token, Arg.Any<CancellationToken>()).ReturnsNull();

            Func<Task> act = () => _service.RemoveRefreshTokenAsync(refreshToken.Token);

            await act.Should().ThrowAsync<InvalidTokenException>();
        }

        [Fact]
        public async Task GetValidTokenEntityAsync_ShouldReturn()
        {
            RefreshToken refreshToken = AuthTestObjects.GetRefreshToken();
            _refreshTokenRepository.GetByToken(refreshToken.Token, Arg.Any<CancellationToken>()).Returns(refreshToken);

            RefreshToken? validToken = await _service.GetValidTokenEntityAsync(refreshToken.Token);

            validToken.Should().NotBeNull();
            validToken!.Token.Should().Be(refreshToken.Token);
        }

        [Fact]
        public async Task GetValidTokenEntityAsync_ShouldReturnNull_WhenTokenDoesNotExist()
        {
            RefreshToken refreshToken = AuthTestObjects.GetRefreshToken();
            _refreshTokenRepository.GetByToken(refreshToken.Token, Arg.Any<CancellationToken>()).ReturnsNull();

            RefreshToken? validToken = await _service.GetValidTokenEntityAsync(refreshToken.Token);

            validToken.Should().BeNull();
        }

        [Fact]
        public async Task GetValidTokenEntityAsync_ShouldReturnNull_WhenTokenIsExpired()
        {
            RefreshToken refreshToken = AuthTestObjects.GetRefreshToken();
            refreshToken.Expires = DateTime.UtcNow.AddMinutes(-1);
            _refreshTokenRepository.GetByToken(refreshToken.Token, Arg.Any<CancellationToken>()).Returns(refreshToken);

            RefreshToken? validToken = await _service.GetValidTokenEntityAsync(refreshToken.Token);

            validToken.Should().BeNull();
        }
    }
}
