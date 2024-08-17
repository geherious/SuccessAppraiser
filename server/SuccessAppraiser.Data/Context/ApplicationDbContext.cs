using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public virtual DbSet<DayState> DayStates { get; set; }
        public virtual DbSet<GoalDate> GoalDates { get; set; }
        public virtual DbSet<GoalItem> GoalItems { get; set; }
        public virtual DbSet<GoalTemplate> GoalTemplates { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

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

            builder.Entity<GoalTemplate>()
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
