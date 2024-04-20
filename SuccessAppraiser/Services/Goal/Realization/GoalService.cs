using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Contracts.Goal;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Goal.Errors;
using SuccessAppraiser.Services.Goal.Interfaces;

namespace SuccessAppraiser.Services.Goal.Realization
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

        public async Task<GoalItem> AddGoalAsync(Guid userId, AddGoalDto addGoalDto, CancellationToken ct = default)
        {
            var template = _dbContext.GoalTemplates.Find(addGoalDto.TemplateId);
            if (template == null)
            {
                throw new InvalidTemplateException();
            }

            GoalItem newGoal = _mapper.Map<GoalItem>(addGoalDto);
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
                throw new GoalNotFoundException();
            }
        }

        public async Task<List<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.GoalItems.Where(g => g.UserId == userId).OrderBy(g => g.DateStart).ToListAsync(ct);
        }

        public async Task<bool> UserhasGoalAsync(Guid userId, Guid goalId, CancellationToken ct = default)
        {
            GoalItem? goal = await _dbContext.GoalItems.FirstOrDefaultAsync(g => (g.UserId == userId) && (g.Id == goalId));
            
            return goal != null;
        }
    }
}
