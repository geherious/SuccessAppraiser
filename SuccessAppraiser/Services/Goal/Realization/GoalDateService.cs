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
        public async Task<GetGoalDateDto> AddGoalDateAsync(AddGoalDateDto dateDto, CancellationToken ct = default)
        {
            var goal = await _dbContext.GoalItems.Where(g => g.Id == dateDto.GoalId).Include(g => g.Dates).Include(g => g.Template).ThenInclude(g => g.States).FirstOrDefaultAsync();

            DateOnly now = DateOnly.FromDateTime(DateTime.UtcNow);

            if (dateDto.Date > now.AddDays(1) || dateDto.Date < goal!.DateStart)
            {
                throw new InvalidDateException();
            }

            if (goal == null)
            {
                throw new GoalNotFoundException();
            }

            if (goal.Dates.Any(d => d.Date == dateDto.Date))
            {
                throw new GoalDateAlreadyExistsException();
            }

            if (!goal.Template.States.Any(s => s.Id == dateDto.StateId))
            {
                throw new InvalidStateException();
            }

            var newGoalDate = _mapper.Map<GoalDate>(dateDto);

            await _dbContext.GoalDates.AddAsync(newGoalDate, ct);
            await _dbContext.SaveChangesAsync(ct);

            var result = _mapper.Map<GetGoalDateDto>(newGoalDate);

            return result;



        }

        public async Task<IList<GetGoalDateDto>> GetGoalDatesByMonthAsync(GetGoalDatesByMonth dateDto, CancellationToken ct = default)
        {
            var dates = await _dbContext.GoalDates.Where(d =>
            d.GoalId == dateDto.GoalId
            && d.Date.Month == dateDto.Date.Month
            && d.Date.Year == dateDto.Date.Year)
            .ToListAsync(ct);

            var result = _mapper.Map<List<GetGoalDateDto>>(dates);

            return result;
        }
    }
}
