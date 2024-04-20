using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Contracts.Goal;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Goal.Errors;
using SuccessAppraiser.Services.Goal.Interfaces;

namespace SuccessAppraiser.Services.Goal.Realization
{
    public class GoalDateService : IGoalDateService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GoalDateService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<GoalDate> AddGoalDateAsync(AddGoalDateDto dateDto, CancellationToken ct = default)
        {
            var goal = await _dbContext.GoalItems.Where(g => g.Id == dateDto.GoalId).Include(g => g.Template).ThenInclude(g => g.States).FirstOrDefaultAsync();
            if (goal == null)
            {
                throw new GoalNotFoundException();
            }

            if (!goal.Template.States.Any(s => s.Id == dateDto.StateId))
            {
                throw new InvalidStateException();
            }

            var newGoalDate = _mapper.Map<GoalDate>(dateDto);

            _dbContext.GoalDates.Add(newGoalDate);
            await _dbContext.SaveChangesAsync();

            return newGoalDate;



        }

        public Task<IList<GetGoalDateDto>> GetGoalDatesByMonth(DateOnly date, Guid goalId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
