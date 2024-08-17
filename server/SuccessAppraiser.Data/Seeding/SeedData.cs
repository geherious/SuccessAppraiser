using Microsoft.Extensions.DependencyInjection;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Seeding.Templates;

namespace SuccessAppraiser.Data.Seeding
{
    public static class SeedData
    {
        public static void PopulateDb(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            AddInitialData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>()!);
        }

        private static void AddInitialData(ApplicationDbContext dbContext)
        {
            SeedTemplate.Seed(dbContext);
        }
    }
}
