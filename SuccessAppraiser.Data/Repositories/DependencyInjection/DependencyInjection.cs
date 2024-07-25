using Microsoft.Extensions.DependencyInjection;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;
using System;

namespace SuccessAppraiser.Data.Repositories.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<IDayStateRepository, DayStateRepository>();
            services.AddScoped<IGoalDateRepository, GoalDateRepository>();
            services.AddScoped<IGoalTemplateRepotitory, GoalTemplateRepository>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            return services;
        }
    }
}
