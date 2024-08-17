using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuccessAppraiser.Api.Goal.Contracts;
using SuccessAppraiser.Api.IntegrationTests.Common;
using SuccessAppraiser.Api.IntegrationTests.TestObjects;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Net;

namespace SuccessAppraiser.Api.IntegrationTests.Goal
{
    public class GoalControllerTest : BaseIntegrationTest, IDisposable
    {
        private readonly IJwtService _jwtService;
        private Guid _baseUserId;
        public GoalControllerTest(ApiWebApplicationFactory factory) : base(factory)
        {
            _baseUserId = _dbContext.ApplicationUsers.First(x => x.UserName.Equals("BaseUser")).Id;
            _jwtService = _scope.ServiceProvider.GetRequiredService<IJwtService>();
            Claim[] userClaims =
            [
                new Claim(ClaimTypes.NameIdentifier, _baseUserId.ToString())
            ];
            var token = _jwtService.GenerateToken(userClaims, Data.Enums.TokenType.AccessToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void Dispose()
        {
            Task.Run(async () => await _dbContext.GoalItems.ExecuteDeleteAsync()).Wait();
            Task.Run(async () => await _dbContext.GoalTemplates.ExecuteDeleteAsync()).Wait();
            Task.Run(async () => await _dbContext.GoalDates.ExecuteDeleteAsync()).Wait();
            Task.Run(async () => await _dbContext.DayStates.ExecuteDeleteAsync()).Wait();
        }

        [Fact]
        public async Task GetGoals_ShouldReturnOne()
        {
            var goal = GoalTestObjects.GetBaseGoal();
            goal.UserId = _baseUserId;
            await _dbContext.GoalItems.AddAsync(goal);
            await _dbContext.SaveChangesAsync();

            var response = await _httpClient.GetAsync("api/goals");

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<List<GetUserGoalDto>>();
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetGoals_ShouldReturnEmptyList()
        {
            var response = await _httpClient.GetAsync("api/goals");

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<List<GetUserGoalDto>>();
            data.Should().HaveCount(0);
        }

        [Fact]
        public async Task CreateGoal_ShouldReturnNewGoal()
        {
            var template = GoalTestObjects.GetABTemplate();
            await _dbContext.GoalTemplates.AddAsync(template);
            await _dbContext.SaveChangesAsync();
            DateOnly start = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            CreateGoalDto dto = new CreateGoalDto("Goal", "Description", 12, start, template.Id);

            var response = await _httpClient.PostAsJsonAsync("api/goals", dto);

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<GetUserGoalDto>();
            data.Should().NotBeNull();
            data!.Name.Should().Be("Goal");
            data.Description.Should().Be("Description");
            data.DaysNumber.Should().Be(12);
            data.DateStart.Should().Be(start);
            data.Template.Id.Should().Be(template.Id);
        }

        [Fact]
        public async Task CreateGoal_ShouldBeBad_WhenTemplateDoesNotExist()
        {
            DateOnly start = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            CreateGoalDto dto = new CreateGoalDto("Goal", "Description", 12, start, Guid.NewGuid());

            var response = await _httpClient.PostAsJsonAsync("api/goals", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public static IEnumerable<object[]> GetValidDatesOfMonth()
        {
            yield return new object[] { "2024-01-01" };
            yield return new object[] { "2024-01-15" };
            yield return new object[] { "2024-01-30" };
        }

        [Theory]
        [MemberData(nameof(GetValidDatesOfMonth))]
        public async Task GetGoalDates_ShouldReturnOne(string date)
        {
            var goal = GoalTestObjects.GetBaseGoal();
            goal.UserId = _baseUserId;
            await _dbContext.GoalItems.AddAsync(goal);
            await _dbContext.SaveChangesAsync();
            var goalDate = GoalTestObjects.GetGoalDate();
            goalDate.GoalId = goal.Id;
            goalDate.StateId = goal.Template!.States.First().Id;
            await _dbContext.GoalDates.AddAsync(goalDate);
            await _dbContext.SaveChangesAsync();

            var response = await _httpClient.GetAsync($"api/goals/{goal.Id}/dates?DateOfMonth={date}");

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<List<GetGoalDateDto>>();
            data.Should().NotBeNull();
            data.Should().HaveCount(1);
        }

        public static IEnumerable<object[]> DatesOfEmptyMonths()
        {
            yield return new object[] { "2024-02-01" };
            yield return new object[] { "2023-12-01" };
            yield return new object[] { "2020-05-11" };
            yield return new object[] { "2040-08-30" };
        }

        [Theory]
        [MemberData(nameof(DatesOfEmptyMonths))]
        public async Task GetGoalDates_ShouldReturnZero(string date)
        {
            var goal = GoalTestObjects.GetBaseGoal();
            goal.UserId = _baseUserId;
            await _dbContext.GoalItems.AddAsync(goal);
            await _dbContext.SaveChangesAsync();
            var goalDate = GoalTestObjects.GetGoalDate();
            goalDate.GoalId = goal.Id;
            goalDate.StateId = goal.Template!.States.First().Id;
            await _dbContext.GoalDates.AddAsync(goalDate);
            await _dbContext.SaveChangesAsync();

            var response = await _httpClient.GetAsync($"api/goals/{goal.Id}/dates?DateOfMonth={date}");

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<List<GetGoalDateDto>>();
            data.Should().NotBeNull();
            data.Should().HaveCount(0);
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnNewGoalDate()
        {
            var goal = GoalTestObjects.GetBaseGoal();
            goal.UserId = _baseUserId;
            await _dbContext.GoalItems.AddAsync(goal);
            await _dbContext.SaveChangesAsync();
            DateOnly date = new DateOnly(2024, 1, 4);
            CreateGoalDateDto dto = new CreateGoalDateDto(date, "Comment", goal.Template.States.First().Id);

            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<GetGoalDateDto>();
            data.Should().NotBeNull();
            data!.Date.Should().Be(date);
            data.Comment.Should().Be("Comment");
            data.StateId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnBad_WhenGoalDoesNotExist()
        {
            var goal = GoalTestObjects.GetBaseGoal();
            goal.UserId = _baseUserId;
            await _dbContext.GoalItems.AddAsync(goal);
            await _dbContext.SaveChangesAsync();
            DateOnly date = new DateOnly(2024, 1, 4);
            CreateGoalDateDto dto = new CreateGoalDateDto(date, "Comment", goal.Template!.States.First().Id);

            var response = await _httpClient.PostAsJsonAsync($"api/goals/{Guid.NewGuid()}/dates", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnBad_WhenStateIsWrong()
        {
            var goal = GoalTestObjects.GetBaseGoal();
            goal.UserId = _baseUserId;
            await _dbContext.GoalItems.AddAsync(goal);
            await _dbContext.SaveChangesAsync();
            DateOnly date = new DateOnly(2024, 1, 4);
            CreateGoalDateDto dto = new CreateGoalDateDto(date, "Comment", Guid.NewGuid());

            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public static IEnumerable<object[]> DatesOutOfRange()
        {
            yield return new object[] { new DateOnly(2020, 1, 1) };
            yield return new object[] { new DateOnly(2024, 2, 1) };
            yield return new object[] { new DateOnly(2024, 1, 13) };
        }

        [Theory]
        [MemberData(nameof(DatesOutOfRange))]
        public async Task CreateGoalDate_ShouldReturnBad_WhenDateIsWrong(DateOnly date)
        {
            var goal = GoalTestObjects.GetBaseGoal();
            goal.UserId = _baseUserId;
            await _dbContext.GoalItems.AddAsync(goal);
            await _dbContext.SaveChangesAsync();
            CreateGoalDateDto dto = new CreateGoalDateDto(date, "Comment", Guid.NewGuid());

            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnBad_WhenDateInFuture()
        {
            var goal = GoalTestObjects.GetBaseGoal();
            goal.DateStart = DateOnly.FromDateTime(DateTime.Now);
            goal.UserId = _baseUserId;
            await _dbContext.GoalItems.AddAsync(goal);
            await _dbContext.SaveChangesAsync();
            DateOnly date = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            CreateGoalDateDto dto = new CreateGoalDateDto(date, "Comment", Guid.NewGuid());

            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
