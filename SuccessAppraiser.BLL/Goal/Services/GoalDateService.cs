using AutoMapper;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using FluentValidation;
using FluentValidation.Results;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.Goal.Exceptions;

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

            if (goal == null)
            {
                throw new InvalidIdException(nameof(GoalItem), createCommand.GoalId);
            }

            ValidateDate(goal, createCommand.Date);

            if (!goal.Template!.States.Any(s => s.Id == createCommand.StateId))
            {
                throw new InvalidIdException(nameof(DayState), createCommand.StateId);
            }

            var newGoalDate = _mapper.Map<GoalDate>(createCommand);

            await _dbContext.GoalDates.AddAsync(newGoalDate, ct);
            await _dbContext.SaveChangesAsync(ct);

            return newGoalDate;
        }

        private void ValidateDate(GoalItem goal, DateOnly date)
        {
            var datesFailures = new List<ValidationFailure>(capacity: 2);
            DateOnly now = DateOnly.FromDateTime(DateTime.UtcNow);

            if (date > now.AddDays(1))
            {
                var message = $"Provided date {date} exeeds current date";
                datesFailures.Add(new ValidationFailure("Date", message));
            }

            if (date < goal.DateStart || date > goal.DateStart.AddDays(goal.DaysNumber))
            {
                var message = $"Provided date should be between the start date and the end date of the goal {goal.DateStart}";
                datesFailures.Add(new ValidationFailure("Date", message));
            }

            if (goal.Dates.Any(d => d.Date == date))
            {
                var message = $"Provided date {date} already exists";
                datesFailures.Add(new ValidationFailure("Date", message));
            }

            if (datesFailures.Count > 0)
            {
                var message = "Provided date is invalid";
                throw new InvalidDateException(message, datesFailures);
            }
        }

        public async Task<IEnumerable<GoalDate>> GetGoalDatesByMonthAsync(GetGoalDatesByMonthQuerry getQuerry, CancellationToken ct = default)
        {
            var goal = await _dbContext.GoalItems.Where(g => g.Id == getQuerry.GoalId).FirstOrDefaultAsync();

            if (goal == null)
            {
                throw new InvalidIdException(nameof(GoalItem), getQuerry.GoalId);
            }

            var dates = await _dbContext.GoalDates.Where(d =>
            d.GoalId == getQuerry.GoalId
            && d.Date.Month == getQuerry.Date.Month
            && d.Date.Year == getQuerry.Date.Year)
            .ToListAsync(ct);

            return dates;
        }
    }
}
