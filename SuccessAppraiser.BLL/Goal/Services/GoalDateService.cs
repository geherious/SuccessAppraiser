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
using SuccessAppraiser.Data.Repositories.Interfaces;
using SuccessAppraiser.Data.Repositories.Base;

namespace SuccessAppraiser.BLL.Goal.Services
{
    public class GoalDateService : IGoalDateService
    {
        private readonly IGoalDateRepository _goalDateRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IGoalTemplateRepotitory _goalTemplateRepotitory;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GoalDateService(IGoalDateRepository goalDateRepository, IGoalRepository goalRepository,
            IGoalTemplateRepotitory goalTemplateRepotitory, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _goalDateRepository = goalDateRepository;
            _goalRepository = goalRepository;
            _goalTemplateRepotitory = goalTemplateRepotitory;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }
        public async Task<GoalDate> CreateGoalDateAsync(CreateGoalDateCommand createCommand, CancellationToken ct = default)
        {
            GoalItem? goal = await _goalRepository.GetByIdAsync(createCommand.GoalId, ct);
            if (goal == null)
            {
                throw new InvalidIdException(nameof(GoalItem), createCommand.GoalId);
            }

            IEnumerable<GoalDate> dates = await _goalDateRepository.FindAsync(x => x.GoalId == createCommand.GoalId, ct);

            ValidateDate(goal, dates, createCommand.Date);

            if (!goal.Template!.States.Any(s => s.Id == createCommand.StateId))
            {
                throw new InvalidIdException(nameof(DayState), createCommand.StateId);
            }

            var newGoalDate = _mapper.Map<GoalDate>(createCommand);

            await _goalDateRepository.AddAsync(newGoalDate, ct);
            await _repositoryWrapper.SaveChangesAsync(ct);

            return newGoalDate;
        }

        private void ValidateDate(GoalItem goal, IEnumerable<GoalDate> dates, DateOnly date)
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

            if (dates.Any(d => d.Date == date))
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
            var goal = await _goalRepository.GetByIdAsync(getQuerry.GoalId, ct);

            if (goal == null)
            {
                throw new InvalidIdException(nameof(GoalItem), getQuerry.GoalId);
            }

            IEnumerable<GoalDate> dates = await _goalDateRepository.FindAsync(d =>
            d.GoalId == getQuerry.GoalId
            && d.Date.Month == getQuerry.DateOfMonth.Month
            && d.Date.Year == getQuerry.DateOfMonth.Year);

            return dates;
        }

        public async Task<GoalDate> UpdateGoaldateAsync(UpdateGoalDateCoomand updateCommand, CancellationToken ct = default)
        {
            GoalItem? goal = await _goalRepository.GetByIdAsync(updateCommand.GoalId, ct);

            if (goal == null)
            {
                throw new InvalidIdException(nameof(GoalItem), updateCommand.GoalId);
            }
            if (!goal.Template!.States.Any(s => s.Id == updateCommand.StateId))
            {
                throw new InvalidIdException(nameof(DayState), updateCommand.StateId);
            }

            IEnumerable<GoalDate> goalDates = await _goalDateRepository
                .FindAsync(d => d.GoalId == updateCommand.GoalId && d.Date == updateCommand.Date, ct);
            GoalDate? goalDate = goalDates.FirstOrDefault();

            if (goalDate == null)
            {
                throw new InvalidDateException(updateCommand.Date);
            }

            goalDate.Comment = updateCommand.Comment;
            goalDate.StateId = updateCommand.StateId;
            await _repositoryWrapper.SaveChangesAsync(ct);

            return goalDate;
        }
    }
}
