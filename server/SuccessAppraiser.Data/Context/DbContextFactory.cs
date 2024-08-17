using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using Testcontainers.PostgreSql;

namespace SuccessAppraiser.Data.Context
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>

    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            PostgreSqlContainer dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("success_appraiser")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

            dbContainer.StartAsync().Wait();

            optionsBuilder.UseNpgsql(dbContainer.GetConnectionString());

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
