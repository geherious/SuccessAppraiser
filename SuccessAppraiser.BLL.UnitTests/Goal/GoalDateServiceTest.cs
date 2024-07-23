using AutoMapper;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Exceptions;
using SuccessAppraiser.BLL.UnitTests.Common;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using SuccessAppraiser.BLL.Goal.Services;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SuccessAppraiser.BLL.UnitTests.Goal
{
    public class GoalDateServiceTest
    {
        private readonly ApplicationDbContext _dbContext = Substitute.For<ApplicationDbContext>();
        private readonly IMapper _mapper;
        private readonly GoalDateService _service;

        public GoalDateServiceTest()
        {
            var goal = GoalObjects.GetHabbitGoal();
            List<GoalItem> goals = new() { goal };
            var goalsMock = goals.AsQueryable().BuildMockDbSet();
            _dbContext.GoalItems.Returns(goalsMock);

            var template = GoalObjects.GetHabbitTemplate();
            List<GoalTemplate> templates = new() { template };
            var templatesMock = templates.AsQueryable().BuildMockDbSet();
            _dbContext.GoalTemplates.Returns(templatesMock);

            var goalDate = GoalObjects.GetHabbitDate11();
            goalDate.GoalId = goal.Id;
            goalDate.Goal = goal;
            List<GoalDate> goalDates = new() { goalDate };
            var goalDatesMock = goalDates.AsQueryable().BuildMockDbSet();
            _dbContext.GoalDates.Returns(goalDatesMock);

            var easy = GoalObjects.GetEasyDayState();
            var average = GoalObjects.GetAverageDayState();
            var hard = GoalObjects.GetHardDayState();
            easy.Templates.Add(template);
            average.Templates.Add(template);
            hard.Templates.Add(template);
            List<DayState> dayStates = new() { easy, average, hard };
            var dayStatesMock = dayStates.AsQueryable().BuildMockDbSet();
            _dbContext.DayStates.Returns(dayStatesMock);

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new GoalServiceMapper()));
            _mapper = new Mapper(mapperConfig);

            _service = new GoalDateService(_dbContext, _mapper);
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
            DayState easy = GoalObjects.GetEasyDayState();
            GoalItem goal = GoalObjects.GetHabbitGoal();
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id);
            command.GoalId = goal.Id;

            GoalDate newDate = await _service.CreateGoalDateAsync(command);

            newDate.Date.Should().Be(command.Date);
            newDate.Comment.Should().Be(command.Comment);
            newDate.StateId.Should().Be(command.StateId);
            newDate.GoalId.Should().Be(command.GoalId);
        }

        [Fact]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenGoalDoesNotExist()
        {
            DayState easy = GoalObjects.GetEasyDayState();
            GoalItem goal = GoalObjects.GetHabbitGoal();
            DateOnly date = new DateOnly(2024, 06, 13);
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id);
            command.GoalId = Guid.NewGuid();

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }

        [Theory]
        [MemberData(nameof(GetInvalidDates))]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenInvalidDates(DateOnly date)
        {
            DayState easy = GoalObjects.GetEasyDayState();
            GoalItem goal = GoalObjects.GetHabbitGoal();
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id);
            command.GoalId = goal.Id;

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidDateException>();
        }

        [Fact]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenDateIsInFuture()
        {
            GoalItem goal = GoalObjects.GetHabbitGoal();
            DateOnly yesterday = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
            goal.DateStart = yesterday;
            List<GoalItem> goals = new() { goal };
            var goalsMock = goals.AsQueryable().BuildMockDbSet();
            _dbContext.GoalItems.Returns(goalsMock);

            DayState easy = GoalObjects.GetEasyDayState();
            DateOnly date = DateOnly.FromDateTime(DateTime.Now).AddDays(2);
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id);
            command.GoalId = goal.Id;

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidDateException>();
        }

        [Fact]
        public async Task UpdateGoaldate_ShouldReturnUpdated()
        {
            var goal = GoalObjects.GetHabbitGoal();
            var goalDate = GoalObjects.GetHabbitDate11();
            var newStateId = GoalObjects.GetEasyDayState().Id;
            UpdateGoalDateCoomand command = new UpdateGoalDateCoomand(goalDate.Date, "New Comment", newStateId);
            command.GoalId = goal.Id;

            var updatedDate = await _service.UpdateGoaldateAsync(command);

            updatedDate.Should().NotBeNull();
            updatedDate.Comment.Should().Be("New Comment");
            updatedDate.StateId.Should().Be(newStateId);
        }

        [Fact]
        public async Task UpdateGoaldate_ShouldThrow_WhenGoalDoesNotExist()
        {
            var goalDate = GoalObjects.GetHabbitDate11();
            var newStateId = GoalObjects.GetEasyDayState().Id;
            UpdateGoalDateCoomand command = new UpdateGoalDateCoomand(goalDate.Date, "New Comment", newStateId);
            command.GoalId = Guid.NewGuid();

            Func<Task> act = () => _service.UpdateGoaldateAsync(command);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }
    }
}
