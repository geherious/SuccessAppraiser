using AutoMapper;
using BLL.Common.Exceptions.Validation;
using BLL.Goal.Contracts;
using BLL.Goal.Exceptions;
using BLL.UnitTests.Common;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;

namespace BLL.UnitTests.Goal
{
    public class GoalDateServiceTest
    {
        private readonly ApplicationDbContext _dbContext = Substitute.For<ApplicationDbContext>();
        private readonly IMapper _mapper;
        private readonly GoalDateService _service;

        public GoalDateServiceTest()
        {
            List<GoalItem> goals = new() { GoalObjects.getHabbitGoal() };
            var goalsMock = goals.AsQueryable().BuildMockDbSet();
            _dbContext.GoalItems.Returns(goalsMock);

            List<GoalTemplate> templates = new() { GoalObjects.getHabbitTemplate() };
            var templatesMock = templates.AsQueryable().BuildMockDbSet();
            _dbContext.GoalTemplates.Returns(templatesMock);

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
            GoalItem goal = GoalObjects.getHabbitGoal();
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id, goal.Id);

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
            GoalItem goal = GoalObjects.getHabbitGoal();
            DateOnly date = new DateOnly(2024, 06, 13);
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id, Guid.NewGuid());

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }

        [Theory]
        [MemberData(nameof(GetInvalidDates))]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenInvalidDates(DateOnly date)
        {
            DayState easy = GoalObjects.GetEasyDayState();
            GoalItem goal = GoalObjects.getHabbitGoal();
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id, goal.Id);

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidDateException>();
        }

        [Fact]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenDateIsInFuture()
        {
            GoalItem goal = GoalObjects.getHabbitGoal();
            DateOnly yesterday = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
            goal.DateStart = yesterday;
            List<GoalItem> goals = new() { goal };
            var goalsMock = goals.AsQueryable().BuildMockDbSet();
            _dbContext.GoalItems.Returns(goalsMock);

            DayState easy = GoalObjects.GetEasyDayState();
            DateOnly date = DateOnly.FromDateTime(DateTime.Now).AddDays(2);
            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", easy.Id, goal.Id);

            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            await act.Should().ThrowAsync<InvalidDateException>();
        }
    }
}
