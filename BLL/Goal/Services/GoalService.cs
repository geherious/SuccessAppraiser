using AutoMapper;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using FluentValidation;
using FluentValidation.Results;

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

        public async Task<GoalItem> CreateGoalAsync(CreateGoalCommand createCommand, CancellationToken ct = default)
        {
            var template = _dbContext.GoalTemplates.Find(createCommand.TemplateId);
            if (template == null)
            {
                var message = $"A template with id {createCommand.TemplateId} doesn't exist";
                throw new ValidationException(message, new[] { new ValidationFailure(nameof(GoalTemplate), message) });
            }

            GoalItem newGoal = _mapper.Map<GoalItem>(createCommand);
            newGoal.Name = newGoal.Name.Trim();
            newGoal.UserId = createCommand.UserId;
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
                var message = $"A goal with id {goalId} doesn't exist";
                throw new ValidationException(message, new[] { new ValidationFailure(nameof(GoalItem), message) });
            }
        }

        public async Task<List<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.GoalItems.Include(g => g.Template).ThenInclude(t => t.States).Where(g => g.UserId == userId).OrderBy(g => g.DateStart).ToListAsync(ct);
        }

        public async Task UserhasGoalOrThrowAsync(Guid userId, Guid goalId, CancellationToken ct = default)
        {
            GoalItem? goal = await _dbContext.GoalItems.FirstOrDefaultAsync(g => g.UserId == userId && g.Id == goalId);

            if (goal  == null)
            {
                var message = $"A goal with id {goalId} was not found";
                throw new ValidationException(message, new[] { new ValidationFailure("Goal id", message) });
            }
        }
    }
}
