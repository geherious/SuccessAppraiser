using SuccessAppraiser.BLL.Auth.Services;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SuccessAppraiser.BLL.Auth
{
    public static class Authregister
    {
        public static void RegisterAuthServices(IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
