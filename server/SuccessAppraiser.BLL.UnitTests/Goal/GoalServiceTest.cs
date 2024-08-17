using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services;
using SuccessAppraiser.BLL.UnitTests.Common;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;

namespace SuccessAppraiser.BLL.UnitTests.Goal
{
    public class GoalServiceTest
    {
        private readonly IGoalRepository _goalRepository = Substitute.For<IGoalRepository>();
        private readonly IGoalTemplateRepotitory _goalTemplateRepotitory = Substitute.For<IGoalTemplateRepotitory>();
        private readonly IRepositoryWrapper _repositoryWrapper = Substitute.For<IRepositoryWrapper>();
        private readonly IMapper _mapper;
        private readonly GoalService _service;

        public GoalServiceTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new GoalServiceMapper()));
            _mapper = new Mapper(mapperConfig);

            _service = new GoalService(_goalRepository, _goalTemplateRepotitory, _repositoryWrapper, _mapper);
        }

        [Fact]
        public async Task CreateGoalAsync_ShouldReturnNewGoal_WhenTemplateExists()
        {
            GoalTemplate template = GoalTestObjects.GetHabbitTemplate();
            CreateGoalCommand command = new CreateGoalCommand(
                "Name", "Description", 12, new DateOnly(2024, 10, 12), template.Id);
            Guid userGuid = Guid.NewGuid();
            command.UserId = userGuid;
            _goalTemplateRepotitory.GetByIdAsync(template.Id, Arg.Any<CancellationToken>()).Returns(template);

            GoalItem goal = await _service.CreateGoalAsync(command);

            goal.Should().NotBeNull();
            goal.Name.Should().Be("Name");
            goal.Description.Should().Be("Description");
            goal.DaysNumber.Should().Be(12);
            goal.DateStart.Should().Be(new DateOnly(2024, 10, 12));
            goal.TemplateId.Should().Be(template.Id);

            await _goalRepository.Received(1)
                .AddAsync(Arg.Any<GoalItem>(), Arg.Any<CancellationToken>());

            await _repositoryWrapper.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateGoalAsync_ShouldThrow_WhenTemplateDoesNotExist()
        {
            GoalTemplate template = GoalTestObjects.GetHabbitTemplate();
            CreateGoalCommand command = new CreateGoalCommand(
                "Name", "Description", 12, new DateOnly(2024, 10, 12), template.Id);
            command.UserId = Guid.NewGuid();
            _goalTemplateRepotitory.GetByIdAsync(template.Id, Arg.Any<CancellationToken>()).ReturnsNull();

            Func<Task> act = () => _service.CreateGoalAsync(command);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalTemplate));
        }

        [Fact]
        public async Task DeleteGoalAsync_ShouldDelete_WhenGoalExists()
        {
            GoalItem goal = GoalTestObjects.GetHabbitGoal();
            _goalRepository.GetByIdAsync(goal.Id, Arg.Any<CancellationToken>()).Returns(goal);

            await _service.DeleteGoalAsync(goal.Id);

            _goalRepository.Received(1)
                .Delete(goal);
            await _repositoryWrapper.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteGoalAsync_ShouldThrow_WhenGoalDoesNotExist()
        {
            GoalItem goal = GoalTestObjects.GetHabbitGoal();
            _goalRepository.GetByIdAsync(goal.Id, Arg.Any<CancellationToken>()).ReturnsNull();

            Func<Task> act = () => _service.DeleteGoalAsync(goal.Id);

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));

        }

        [Fact]
        public async Task UserHasGoalAsync_ShouldBeOk_WhenGoalExists()
        {
            GoalItem goal = GoalTestObjects.GetHabbitGoal();
            Guid userId = Guid.Parse("cffaea27-8a8f-471d-aa12-39913ffbbda3");
            _goalRepository.UserHasGoalAsync(userId, goal.Id, Arg.Any<CancellationToken>()).Returns(true);

            await _service.UserhasGoalOrThrowAsync(userId, goal.Id);
        }

        [Fact]
        public async Task UserHasGoalAsync_ShouldThrow_WhenGoalDoesNotExist()
        {
            GoalItem goal = GoalTestObjects.GetHabbitGoal();
            Guid userId = Guid.Parse("cffaea27-8a8f-471d-aa12-39913ffbbda3");
            _goalRepository.UserHasGoalAsync(userId, goal.Id, Arg.Any<CancellationToken>()).Returns(false);

            Func<Task> act = () => _service.UserhasGoalOrThrowAsync(userId, Guid.NewGuid());

            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }
    }

}
