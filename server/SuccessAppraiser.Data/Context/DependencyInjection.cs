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
                options.UseNpgsql(configuration.GetConnectionString("DockerDb"));
            });
            return services;
        }
    }
}
