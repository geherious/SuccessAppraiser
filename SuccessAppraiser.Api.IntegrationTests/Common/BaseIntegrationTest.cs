using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SuccessAppraiser.Api.IntegrationTests.TestObjects;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Api.IntegrationTests.Common
{
    public abstract class BaseIntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly IServiceScope _scope;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly HttpClient _httpClient;
        private readonly UserManager<ApplicationUser> _userManager;

        protected BaseIntegrationTest(ApiWebApplicationFactory factory)
        {
            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _httpClient = factory.CreateClient();
            _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            Task.Run(RegisterBaseUser).Wait();

        }

        private async Task RegisterBaseUser()
        {
            ApplicationUser user = AuthTestObjects.getBaseUser();
            string password = "Password123";

            await _userManager.CreateAsync(user, password);
        }
    }
}
