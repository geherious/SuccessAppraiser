using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;

namespace SuccessAppraiser.Data.Repositories
{
    public class GoalTemplateRepository : BaseRepository<GoalTemplate>, IGoalTemplateRepotitory
    {
        public GoalTemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<GoalTemplate?> GetByIdAsync(Guid guid, CancellationToken ct = default)
        {
            return _dbSet.Include(t => t.States).FirstOrDefaultAsync(t => t.Id == guid, ct);
        }
    }
}
