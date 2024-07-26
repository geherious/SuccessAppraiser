using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Exceptions;
using SuccessAppraiser.BLL.Goal.Services;
using SuccessAppraiser.BLL.UnitTests.Common;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SuccessAppraiser.BLL.UnitTests.Goal
{
    public class GoalDateServiceTest
    {
        private readonly IGoalRepository _goalRepository = Substitute.For<IGoalRepository>();
        private readonly IGoalDateRepository _goalDateRepository = Substitute.For<IGoalDateRepository>();
        private readonly IRepositoryWrapper _repositoryWrapper = Substitute.For<IRepositoryWrapper>();
        private readonly IGoalTemplateRepotitory _goalTemplateRepotitory = Substitute.For<IGoalTemplateRepotitory>();
        private readonly IMapper _mapper;
        private readonly GoalDateService _service;

        public GoalDateServiceTest()
        {

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new GoalServiceMapper()));
            _mapper = new Mapper(mapperConfig);

            _service = new GoalDateService(_goalDateRepository, _goalRepository, _goalTemplateRepotitory,
                _repositoryWrapper, _mapper);
        }

        public static IEnumerable<object[]> GetValidDates_StatingFrom12To24()
        {
            yield return new object[] { new DateOnly(2024, 06, 12) };
            yield return new object[] { new DateOnly(2024, 06, 20) };
            yield return new object[] { new DateOnly(2024, 06, 24) };
        }

        public static IEnumerable<object[]> GetInvalidDates()
        {
            DateTime now = DateTime.Now;
            yield return new object[] { new DateOnly(2024, 06, 11) };
            yield return new object[] { new DateOnly(2024, 06, 25) };
        }

        [Theory]
        [MemberData(nameof(GetValidDates_StatingFrom12To24))]
        public async Task CreateGoalDateAsync_ShouldReturnNewDate(DateOnly date)
        {
            DayState easy = GoalTestObjects.GetEasyDayState();
            GoalItem goal = GoalTestObjects.GetHabbitGoal();
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id);
            command.GoalId = goal.Id;
            _goalRepository.GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>()).Returns(goal);
            _goalDateRepository.FindAsync(x => x.GoalId == command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal.Dates);

            GoalDate newDate = await _service.CreateGoalDateAsync(command);

            newDate.Date.Should().Be(command.Date);
            newDate.Comment.Should().Be(command.Comment);
            newDate.StateId.Should().Be(command.StateId);
            newDate.GoalId.Should().Be(command.GoalId);

            await _goalDateRepository.Received(1)
                .AddAsync(newDate, Arg.Any<CancellationToken>());
            await _repositoryWrapper.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenGoalDoesNotExist()
        {
            DayState easy = GoalTestObjects.GetEasyDayState();
            GoalItem goal = GoalTestObjects.GetHabbitGoal();
            DateOnly date = new DateOnly(2024, 06, 13);
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id);
            command.GoalId = Guid.NewGuid();
            _goalRepository.GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>()).ReturnsNull();

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }

        [Theory]
        [MemberData(nameof(GetInvalidDates))]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenInvalidDates(DateOnly date)
        {
            DayState easy = GoalTestObjects.GetEasyDayState();
            GoalItem goal = GoalTestObjects.GetHabbitGoal();
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id);
            command.GoalId = goal.Id;
            _goalRepository.GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>()).Returns(goal);
            _goalDateRepository.FindAsync(x => x.GoalId == command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal.Dates);

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidDateException>();
        }

        [Fact]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenDateIsInFuture()
        {
            GoalItem goal = GoalTestObjects.GetHabbitGoal();
            DateOnly yesterday = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
            goal.DateStart = yesterday;
            DayState easy = GoalTestObjects.GetEasyDayState();
            DateOnly date = DateOnly.FromDateTime(DateTime.Now).AddDays(2);
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id);
            command.GoalId = goal.Id;
            _goalRepository.GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>()).Returns(goal);
            _goalDateRepository.FindAsync(x => x.GoalId == command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal.Dates);

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidDateException>();
        }

        [Fact]
        public async Task UpdateGoaldate_ShouldReturnUpdated()
        {
            var goal = GoalTestObjects.GetHabbitGoal();
            var goalDate = GoalTestObjects.GetHabbitDate11();
            var newStateId = GoalTestObjects.GetEasyDayState().Id;
            UpdateGoalDateCoomand command = new UpdateGoalDateCoomand(goalDate.Date, "New Comment", newStateId);
            command.GoalId = goal.Id;
            _goalRepository.GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>()).Returns(goal);
            _goalDateRepository
                .FindAsync(Arg.Any<Expression<Func<GoalDate, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(new[] { goalDate });

            var updatedDate = await _service.UpdateGoaldateAsync(command);

            updatedDate.Should().NotBeNull();
            updatedDate.Comment.Should().Be("New Comment");
            updatedDate.StateId.Should().Be(newStateId);
        }

        [Fact]
        public async Task UpdateGoaldate_ShouldThrow_WhenGoalDoesNotExist()
        {
            var goalDate = GoalTestObjects.GetHabbitDate11();
            var newStateId = GoalTestObjects.GetEasyDayState().Id;
            UpdateGoalDateCoomand command = new UpdateGoalDateCoomand(goalDate.Date, "New Comment", newStateId);
            command.GoalId = Guid.NewGuid();
            _goalRepository.GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>()).ReturnsNull();

            Func<Task> act = () => _service.UpdateGoaldateAsync(command);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }

        [Fact]
        public async Task UpdateGoaldate_ShouldThrow_WhenGoalDateDoesNotExist()
        {
            var goal = GoalTestObjects.GetHabbitGoal();
            var goalDate = GoalTestObjects.GetHabbitDate11();
            var newStateId = GoalTestObjects.GetEasyDayState().Id;
            UpdateGoalDateCoomand command = new UpdateGoalDateCoomand(goalDate.Date, "New Comment", newStateId);
            command.GoalId = Guid.NewGuid();
            _goalRepository.GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>()).Returns(goal);
            _goalDateRepository
                .FindAsync(Arg.Any<Expression<Func<GoalDate, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(Array.Empty<GoalDate>());

            Func<Task> act = () => _service.UpdateGoaldateAsync(command);

            await act.Should().ThrowAsync<InvalidDateException>();
        }
    }
}
