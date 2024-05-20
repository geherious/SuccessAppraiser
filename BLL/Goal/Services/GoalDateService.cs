using AutoMapper;
using SuccessAppraiser.BLL.Common.Exceptions;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.Goal.Services
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
        public async Task<GoalDate> CreateGoalDateAsync(CreateGoalDateCommand createCommand, CancellationToken ct = default)
        {
            var goal = await _dbContext.GoalItems.Where(g => g.Id == createCommand.GoalId).Include(g => g.Dates).Include(g => g.Template).ThenInclude(g => g.States).FirstOrDefaultAsync();

            DateOnly now = DateOnly.FromDateTime(DateTime.UtcNow);
            if (goal == null)
            {
                throw new NotFoundException(nameof(createCommand.GoalId));
            }

            if (createCommand.Date > now.AddDays(1))
            {
                throw new ArgumentException("Provided date exeeds current date");
            }

            if (createCommand.Date < goal.DateStart || createCommand.Date > goal.DateStart.AddDays(goal.DaysNumber))
            {
                throw new ArgumentException("Provided date should be between the start date and the end date of the goal",
                    nameof(createCommand.Date));
            }


            if (goal.Dates.Any(d => d.Date == createCommand.Date))
            {
                throw new ArgumentException("Provided date already exists", nameof(createCommand.Date));
            }

            if (!goal.Template.States.Any(s => s.Id == createCommand.StateId))
            {
                throw new NotFoundException(nameof(createCommand.StateId));
            }

            var newGoalDate = _mapper.Map<GoalDate>(createCommand);

            await _dbContext.GoalDates.AddAsync(newGoalDate, ct);
            await _dbContext.SaveChangesAsync(ct);

            return newGoalDate;



        }

        public async Task<IEnumerable<GoalDate>> GetGoalDatesByMonthAsync(GetGoalDatesByMonthQuerry getQuerry, CancellationToken ct = default)
        {
            var dates = await _dbContext.GoalDates.Where(d =>
            d.GoalId == getQuerry.GoalId
            && d.Date.Month == getQuerry.Date.Month
            && d.Date.Year == getQuerry.Date.Year)
            .ToListAsync(ct);

            return dates;
        }
    }
}
