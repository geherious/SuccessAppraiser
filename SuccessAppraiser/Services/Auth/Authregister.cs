using SuccessAppraiser.Services.Auth.Interfaces;
using SuccessAppraiser.Services.Auth.Realization;


namespace SuccessAppraiser.Services.Auth
{
    public static class Authregister
    {
        public static void RegisterAuthServices(IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
