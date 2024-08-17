using SuccessAppraiser.BLL.Goal.Services;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SuccessAppraiser.BLL.Goal
{
    public static class GoalRegister
    {
        public static void RegisterGoalServices(IServiceCollection services)
        {
            services.AddScoped<IGoalService, GoalService>();
            services.AddScoped<IGoalDateService, GoalDateService>();
        }
    }
}
