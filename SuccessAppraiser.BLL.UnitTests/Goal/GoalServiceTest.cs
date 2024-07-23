using AutoMapper;
using SuccessAppraiser.BLL.Goal.Contracts;
using NSubstitute;
using SuccessAppraiser.BLL.Goal.Services;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using MockQueryable.NSubstitute;
using NSubstitute.ReturnsExtensions;
using FluentAssertions;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.UnitTests.Common;

namespace SuccessAppraiser.BLL.UnitTests.Goal
{
    public class GoalServiceTest
    {
        private readonly ApplicationDbContext _dbContext = Substitute.For<ApplicationDbContext>();
        private readonly IMapper _mapper;
        private readonly GoalService _service;

        public GoalServiceTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new GoalServiceMapper()));
            _mapper = new Mapper(mapperConfig);

            _service = new GoalService(_dbContext, _mapper);
        }

        [Fact]
        public async Task CreateGoalAsync_ShouldReturnNewGoal_WhenTemplateExists()
        {
            GoalTemplate template = GoalObjects.GetHabbitTemplate();
            CreateGoalCommand command = new CreateGoalCommand(
                "Name", "Description", 12, new DateOnly(2024, 10, 12), template.Id);
            Guid userGuid = Guid.NewGuid();
            command.UserId = userGuid;
            _dbContext.GoalTemplates.FindAsync(template.Id, Arg.Any<CancellationToken>()).Returns(template);

            GoalItem goal = await _service.CreateGoalAsync(command);

            goal.Should().NotBeNull();
            goal.Name.Should().Be("Name");
            goal.Description.Should().Be("Description");
            goal.DaysNumber.Should().Be(12);
            goal.DateStart.Should().Be(new DateOnly(2024, 10, 12));
            goal.TemplateId.Should().Be(template.Id);

            await _dbContext.GoalItems.Received(1)
                .AddAsync(Arg.Any<GoalItem>(), Arg.Any<CancellationToken>());

            await _dbContext.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateGoalAsync_ShouldThrow_WhenTemplateDoesNotExist()
        {
            GoalTemplate template = GoalObjects.GetHabbitTemplate();
            CreateGoalCommand command = new CreateGoalCommand(
                "Name", "Description", 12, new DateOnly(2024, 10, 12), template.Id);
            Guid userGuid = Guid.NewGuid();
            command.UserId = userGuid;
            _dbContext.GoalTemplates.FindAsync(template.Id).ReturnsNull();

            Func<Task> act = () => _service.CreateGoalAsync(command);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalTemplate));
        }

        [Fact]
        public async Task DeleteGoalAsync_ShouldDelete_WhenGoalExists()
        {
            GoalItem goal = GoalObjects.GetHabbitGoal();
            _dbContext.GoalItems.FindAsync(goal.Id, Arg.Any<CancellationToken>()).Returns(goal);

            await _service.DeleteGoalAsync(goal.Id);

            _dbContext.GoalItems.Received(1)
                .Remove(goal);
            await _dbContext.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteGoalAsync_ShouldThrow_WhenGoalDoesNotExist()
        {
            GoalItem goal = GoalObjects.GetHabbitGoal();
            _dbContext.GoalItems.FindAsync(goal.Id, Arg.Any<CancellationToken>()).ReturnsNull();

            Func<Task> act = () => _service.DeleteGoalAsync(goal.Id);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));

        }

        [Fact]
        public async Task UserHasGoalAsync_ShouldBeOk_WhenGoalExists()
        {
            GoalItem goal = GoalObjects.GetHabbitGoal();
            Guid userId = Guid.Parse("cffaea27-8a8f-471d-aa12-39913ffbbda3");
            BuildGoalMock(goal);

            await _service.UserhasGoalOrThrowAsync(userId, goal.Id);
        }

        private void BuildGoalMock(GoalItem? goal = null)
        {
            List<GoalItem> goalList = new();
            if (goal != null)
            {
                goalList.Add(goal);
            }
            var goalsMock = goalList.AsQueryable().BuildMockDbSet();
            _dbContext.GoalItems.Returns(goalsMock);
        }

        [Fact]
        public async Task UserHasGoalAsync_ShouldThrow_WhenGoalDoesNotExist()
        {
            GoalItem goal = GoalObjects.GetHabbitGoal();
            Guid userId = Guid.Parse("cffaea27-8a8f-471d-aa12-39913ffbbda3");
            BuildGoalMock(goal);

            Func<Task> act = () => _service.UserhasGoalOrThrowAsync(userId, Guid.NewGuid());

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }
    }

}
