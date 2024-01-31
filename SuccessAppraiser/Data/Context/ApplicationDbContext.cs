using System.Net.Mime;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<DayState> DayStates { get; set; }
        public DbSet<GoalDate> GoalDates { get; set; }
        public DbSet<GoalItem> GoalItems { get; set; }
        public DbSet<GoalTemplate> GoalTemplates { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
