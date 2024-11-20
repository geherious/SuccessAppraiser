using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services;
using SuccessAppraiser.BLL.UnitTests.Fakers;
using SuccessAppraiser.BLL.UnitTests.TestObjects;
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
            // arrange
            GoalTemplate template = TemplateFaker.Generate(1)[0];

            CreateGoalCommand command = new CreateGoalCommand
            {
                Name = "Name",
                Description = "Description",
                DaysNumber = 12,
                DateStart = new DateOnly(2024, 10, 12),
                TemplateId = template.Id
            };

            Guid userGuid = Guid.NewGuid();
            command.UserId = userGuid;

            _goalTemplateRepotitory
                .GetByIdAsync(template.Id, Arg.Any<CancellationToken>())
                .Returns(template);

            // act
            GoalItem goal = await _service.CreateGoalAsync(command);

            // assert
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
            // arrange
            GoalTemplate template = TemplateFaker.Generate(1)[0];

            CreateGoalCommand command = new CreateGoalCommand
            {
                Name = "Name",
                Description = "Description",
                DaysNumber = 12,
                DateStart = new DateOnly(2024, 10, 12),
                TemplateId = template.Id
            };

            command.UserId = Guid.NewGuid();

            _goalTemplateRepotitory.GetByIdAsync(template.Id, Arg.Any<CancellationToken>()).ReturnsNull();

            // act
            Func<Task> act = () => _service.CreateGoalAsync(command);

            // assert
            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalTemplate));
        }

        [Fact]
        public async Task DeleteGoalAsync_ShouldDelete_WhenGoalExists()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            _goalRepository.GetByIdAsync(goal.Id, Arg.Any<CancellationToken>()).Returns(goal);

            // act
            await _service.DeleteGoalAsync(goal.Id);

            // assert
            _goalRepository.Received(1)
                .Delete(goal);
            await _repositoryWrapper.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteGoalAsync_ShouldThrow_WhenGoalDoesNotExist()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            _goalRepository.GetByIdAsync(goal.Id, Arg.Any<CancellationToken>()).ReturnsNull();

            // act
            Func<Task> act = () => _service.DeleteGoalAsync(goal.Id);

            // assert
            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));

        }

        [Fact]
        public async Task UserHasGoalAsync_ShouldBeOk_WhenGoalExists()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            _goalRepository.UserHasGoalAsync(goal.UserId, goal.Id, Arg.Any<CancellationToken>()).Returns(true);

            // act
            await _service.UserhasGoalOrThrowAsync(goal.UserId, goal.Id);
        }

        [Fact]
        public async Task UserHasGoalAsync_ShouldThrow_WhenGoalDoesNotExist()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            _goalRepository.UserHasGoalAsync(goal.UserId, goal.Id, Arg.Any<CancellationToken>()).Returns(false);

            // act
            Func<Task> act = () => _service.UserhasGoalOrThrowAsync(goal.UserId, goal.Id);

            // assert
            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }
    }
}