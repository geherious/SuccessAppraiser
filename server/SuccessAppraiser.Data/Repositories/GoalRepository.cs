using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;

namespace SuccessAppraiser.Data.Repositories
{
    public class GoalRepository : BaseRepository<GoalItem>, IGoalRepository
    {
        public GoalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<GoalItem?> GetByIdAsync(Guid guid, CancellationToken ct = default)
        {
            return _dbSet.Include(g => g.Template)
                .ThenInclude(t => t!.States)
                .FirstOrDefaultAsync(g => g.Id == guid, ct);
        }

        public async Task<IEnumerable<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbSet.Include(g => g.Template).ThenInclude(t => t.States).Where(g => g.UserId == userId).OrderBy(g => g.DateStart).ToListAsync(ct);
        }

        public async Task<bool> UserHasGoalAsync(Guid userId, Guid goalId, CancellationToken ct = default)
        {
            GoalItem? goal = await _dbSet.FirstOrDefaultAsync(g => g.UserId == userId && g.Id == goalId, ct);
            return goal != null;

        }
    }
}
