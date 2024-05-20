using AutoMapper;
using SuccessAppraiser.BLL.Common.Exceptions;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.Goal.Services
{
    public class GoalService : IGoalService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GoalService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GoalItem> CreateGoalAsync(Guid userId, CreateGoalCommand createCommand, CancellationToken ct = default)
        {
            var template = _dbContext.GoalTemplates.Find(createCommand.TemplateId);
            if (template == null)
            {
                throw new NotFoundException(nameof(createCommand.TemplateId));
            }

            GoalItem newGoal = _mapper.Map<GoalItem>(createCommand);
            newGoal.UserId = userId;
            await _dbContext.GoalItems.AddAsync(newGoal, ct);
            await _dbContext.SaveChangesAsync(ct);
            return newGoal;
        }

        public async Task DeleteGoalAsync(Guid goalId, CancellationToken ct = default)
        {
            GoalItem? goal = await _dbContext.GoalItems.FindAsync(goalId, ct);
            if (goal != null)
            {
                _dbContext.GoalItems.Remove(goal);
                await _dbContext.SaveChangesAsync(ct);
            }
            else
            {
                throw new NotFoundException(nameof(goalId));
            }
        }

        public async Task<List<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.GoalItems.Include(g => g.Template).ThenInclude(t => t.States).Where(g => g.UserId == userId).OrderBy(g => g.DateStart).ToListAsync(ct);
        }

        public async Task<bool> UserhasGoalAsync(Guid userId, Guid goalId, CancellationToken ct = default)
        {
            GoalItem? goal = await _dbContext.GoalItems.FirstOrDefaultAsync(g => g.UserId == userId && g.Id == goalId);

            return goal != null;
        }
    }
}
