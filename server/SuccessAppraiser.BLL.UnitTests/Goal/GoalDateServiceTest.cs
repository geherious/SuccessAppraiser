using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Exceptions;
using SuccessAppraiser.BLL.Goal.Services;
using SuccessAppraiser.BLL.UnitTests.Fakers;
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

        [Fact]
        public async Task CreateGoalDateAsync_ShouldReturnNewDate()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            DayState state = goal.Template!.States[0];
            DateOnly date = DateFaker
                .GetRandomDateTime(goal.DateStart, goal.DateStart.AddDays(goal.DaysNumber));

            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", state.Id);
            command.GoalId = goal.Id;

            _goalRepository
                .GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal);
            _goalDateRepository
                .FindAsync(x => x.GoalId == command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal.Dates);

            // act
            GoalDate newDate = await _service.CreateGoalDateAsync(command);

            // assert
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
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            DayState state = goal.Template!.States[0];
            DateOnly date = DateFaker
                .GetRandomDateTime(goal.DateStart, goal.DateStart.AddDays(goal.DaysNumber));

            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", state.Id);
            command.GoalId = Guid.NewGuid();

            _goalRepository
                .GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // act
            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            // assert
            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }

        [Fact]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenDateIsSmallerThanStart()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            DayState state = goal.Template!.States[0];
            DateOnly date = goal.DateStart.AddDays(-1);

            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", state.Id);
            command.GoalId = goal.Id;

            _goalRepository
                .GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal);
            _goalDateRepository
                .FindAsync(x => x.GoalId == command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal.Dates);

            // act
            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            // assert
            await act.Should().ThrowAsync<InvalidDateException>();
        }

        [Fact]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenDateIsBiggerThanEnd()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            DayState state = goal.Template!.States[0];
            DateOnly date = goal.DateStart.AddDays(goal.DaysNumber + 1);

            CreateGoalDateCommand command = new CreateGoalDateCommand(date, "Comment", state.Id);
            command.GoalId = goal.Id;

            _goalRepository
                .GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal);
            _goalDateRepository
                .FindAsync(x => x.GoalId == command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal.Dates);

            // act
            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            // assert
            await act.Should().ThrowAsync<InvalidDateException>();
        }

        [Fact]
        public async Task CreateGoalDateAsync_ShouldThrow_WhenDateIsInFuture()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            DayState state = goal.Template!.States[0];

            DateOnly yesterday = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
            DateOnly futureDate = DateOnly.FromDateTime(DateTime.Now).AddDays(2);

            goal.DateStart = yesterday;

            CreateGoalDateCommand command = new CreateGoalDateCommand(futureDate, "Comment", state.Id);
            command.GoalId = goal.Id;

            _goalRepository
                .GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal);
            _goalDateRepository
                .FindAsync(x => x.GoalId == command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal.Dates);

            // act
            Func<Task> act = () => _service.CreateGoalDateAsync(command);

            // assert
            await act.Should().ThrowAsync<InvalidDateException>();
        }

        [Fact]
        public async Task UpdateGoaldate_ShouldReturnUpdated()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            DayState state = goal.Template!.States[0];

            GoalDate goalDate = goal.AddRandomDate();

            var newStateId = goal.Template!.States[1].Id;

            UpdateGoalDateCoomand command = new UpdateGoalDateCoomand(goalDate.Date, "New Comment", newStateId);
            command.GoalId = goal.Id;

            _goalRepository
                .GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal);
            _goalDateRepository
                .FindAsync(Arg.Any<Expression<Func<GoalDate, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(new[] { goalDate });

            // act
            var updatedDate = await _service.UpdateGoaldateAsync(command);

            // assert
            updatedDate.Should().NotBeNull();
            updatedDate.Comment.Should().Be("New Comment");
            updatedDate.StateId.Should().Be(newStateId);
        }

        [Fact]
        public async Task UpdateGoaldate_ShouldThrow_WhenGoalDoesNotExist()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            DayState state = goal.Template!.States[0];

            GoalDate goalDate = goal.AddRandomDate();

            UpdateGoalDateCoomand command = new UpdateGoalDateCoomand(goalDate.Date, "New Comment", state.Id);
            command.GoalId = goal.Id;

            _goalRepository
                .GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // act
            Func<Task> act = () => _service.UpdateGoaldateAsync(command);

            // assert
            await act.Should().ThrowAsync<InvalidIdException>()
                .Where(e => e.ClassName == nameof(GoalItem));
        }

        [Fact]
        public async Task UpdateGoaldate_ShouldThrow_WhenGoalDateDoesNotExist()
        {
            // arrange
            GoalItem goal = GoalFaker.Generate(1)[0];
            DayState state = goal.Template!.States[0];

            GoalDate goalDate = goal.AddRandomDate();
            goal.Dates.Clear();

            UpdateGoalDateCoomand command = new UpdateGoalDateCoomand(goalDate.Date, "New Comment", state.Id);
            command.GoalId = Guid.NewGuid();

            _goalRepository
                .GetByIdAsync(command.GoalId, Arg.Any<CancellationToken>())
                .Returns(goal);
            _goalDateRepository
                .FindAsync(Arg.Any<Expression<Func<GoalDate, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(Array.Empty<GoalDate>());

            // act
            Func<Task> act = () => _service.UpdateGoaldateAsync(command);

            // assert
            await act.Should().ThrowAsync<InvalidDateException>();
        }
    }
}