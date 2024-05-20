using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<DayState> DayStates { get; set; }
        public DbSet<GoalDate> GoalDates { get; set; }
        public DbSet<GoalItem> GoalItems { get; set; }
        public DbSet<GoalTemplate> GoalTemplates { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RefreshToken>()
                .HasIndex(x => x.Token)
                .IsUnique();

            builder.Entity<GoalDate>()
                .HasIndex(x => new { x.GoalId, x.Date })
                .IsUnique();
        }
    }
}
