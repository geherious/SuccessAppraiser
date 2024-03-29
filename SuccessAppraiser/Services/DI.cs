﻿using SuccessAppraiser.Services.Auth;
using SuccessAppraiser.Services.Goal;

namespace SuccessAppraiser.Services
{
    public static class DI
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            GoalRegister.RegisterGoalServices(services);
            Authregister.RegisterAuthServices(services);
            return services;
        }
    }
}
