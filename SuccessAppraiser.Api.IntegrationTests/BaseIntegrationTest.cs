using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using SuccessAppraiser.Data.Context;
using System;

namespace Api.IntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly ApplicationDbContext DbContext;

        protected BaseIntegrationTest(ApiWebApplicationFactory factory)
        {
            _scope = factory.Services.CreateScope();
            DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }
}
