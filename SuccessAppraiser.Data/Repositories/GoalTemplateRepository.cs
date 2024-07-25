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
    }
}
