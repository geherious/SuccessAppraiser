using SuccessAppraiser.Services.Goal.Interfaces;
using SuccessAppraiser.Services.Goal.Realization;

namespace SuccessAppraiser.Services.Goal
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
