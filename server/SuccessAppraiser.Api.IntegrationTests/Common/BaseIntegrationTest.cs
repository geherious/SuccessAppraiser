
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SuccessAppraiser.Api.Auth.Contracts;
using SuccessAppraiser.Api.IntegrationTests.Fakers;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using System.Net.Http.Json;
using System.Security.Claims;

namespace SuccessAppraiser.Api.IntegrationTests.Common
{
    public abstract class BaseIntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly HttpClient _httpClient;
        protected readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;

        protected BaseIntegrationTest(ApiWebApplicationFactory factory)
        {
            IServiceScope scope = factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _httpClient = factory.CreateClient();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
        }

        protected Guid CreateNewUser()
        {
            RegisterDto dto = RegisterDtoFaker.Generate();
            _httpClient.PostAsJsonAsync("api/auth/register", dto).GetAwaiter().GetResult();

            Guid userId = _dbContext.ApplicationUsers.First(x => x.UserName!.Equals(dto.Username)).Id;
            return userId;
        }

        protected string GetTokenForNewUser(Guid userId)
        {
            Claim[] userClaims =
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            ];

            return _jwtService.GenerateToken(userClaims, Data.Enums.TokenType.AccessToken);
        }
    }
}