using Microsoft.EntityFrameworkCore;

namespace SuccessAppraiser.Data.Context
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfiguredDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("WebDb"));
            });
            return services;
        }
    }
}
