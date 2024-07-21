using Microsoft.Extensions.DependencyInjection;
using SuccessAppraiser.Data.Context;

namespace SuccessAppraiser.Api.IntegrationTests.Common
{
    public abstract class BaseIntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly IServiceScope _scope;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly HttpClient _httpClient;

        protected BaseIntegrationTest(ApiWebApplicationFactory factory)
        {
            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _httpClient = factory.CreateClient();
        }
    }
}
