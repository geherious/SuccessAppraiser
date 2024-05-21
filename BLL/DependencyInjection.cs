using SuccessAppraiser.BLL.Auth;
using SuccessAppraiser.BLL.Goal;
using Microsoft.Extensions.DependencyInjection;

namespace SuccessAppraiser.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            GoalRegister.RegisterGoalServices(services);
            Authregister.RegisterAuthServices(services);
            return services;
        }
    }
}
