using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;

namespace SuccessAppraiser.Data.Repositories
{
    public class GoalDateRepository : BaseRepository<GoalDate>, IGoalDateRepository
    {
        public GoalDateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
