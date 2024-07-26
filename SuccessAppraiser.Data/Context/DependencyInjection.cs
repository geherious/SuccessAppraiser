using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace SuccessAppraiser.Data.Context
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfiguredDbContext(this IServiceCollection services, string environmnetName, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (environmnetName.Equals("DEVELOPMENT", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseNpgsql(configuration.GetConnectionString("LocalDb"));
                }
                else
                {
                    options.UseNpgsql(configuration.GetConnectionString("WebDb"));

                }
            });
            return services;
        }
    }
}
