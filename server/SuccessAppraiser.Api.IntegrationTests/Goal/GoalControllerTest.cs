using FluentAssertions;
using SuccessAppraiser.Api.Goal.Contracts;
using SuccessAppraiser.Api.IntegrationTests.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using SuccessAppraiser.Api.IntegrationTests.Fakers;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Api.IntegrationTests.Goal
{
    public class GoalControllerTest : BaseIntegrationTest
    {
        private readonly GetRawTemplateDto _template;
        public GoalControllerTest(ApiWebApplicationFactory factory) : base(factory)
        {
            Guid userId = CreateNewUser();
            var token = GetTokenForNewUser(userId);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var templateDto = CreateTemplateDtoFaker.Generate();
            var templateResponse = _httpClient.PostAsJsonAsync("api/templates", templateDto).GetAwaiter().GetResult();
            _template = templateResponse.Content.ReadFromJsonAsync<GetRawTemplateDto>().GetAwaiter().GetResult()!;
        }

        [Fact]
        public async Task CreateGoal_ShouldReturnNewGoal()
        {
            // arrange
            CreateGoalDto dto = CreateGoalDtoFaker.Generate(_template.Id);

            // act
            var response = await _httpClient.PostAsJsonAsync("api/goals", dto);

            // assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<GetUserGoalDto>();
            data.Should().NotBeNull();
            
            data!.Name.Should().Be(dto.Name);
            data.Description.Should().Be(dto.Description);
            data.DaysNumber.Should().Be(dto.DaysNumber);
            data.DateStart.Should().Be(dto.DateStart);
            data.Template.Id.Should().Be(_template.Id);
            data.Template.States.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task CreateGoal_ShouldBeBad_WhenTemplateDoesNotExist()
        {
            // arrange
            CreateGoalDto dto = CreateGoalDtoFaker.Generate(Guid.NewGuid());

            // act
            var response = await _httpClient.PostAsJsonAsync("api/goals", dto);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetGoals_ShouldReturnOne()
        {
            // arrange
            CreateGoalDto dto = CreateGoalDtoFaker.Generate(_template.Id);
            await _httpClient.PostAsJsonAsync("api/goals", dto);

            // act
            var response = await _httpClient.GetAsync("api/goals");

            // assert
            response.EnsureSuccessStatusCode();
            
            var data = await response.Content.ReadFromJsonAsync<List<GetUserGoalDto>>();
            data.Should().NotBeNull();
            data.Should().HaveCount(1);

            data!.Single().Name.Should().Be(dto.Name);
            data!.Single().DaysNumber.Should().Be(dto.DaysNumber);
            data!.Single().Template.Id.Should().Be(_template.Id);
            data!.Single().DateStart.Should().Be(dto.DateStart);
        }

        [Fact]
        public async Task GetGoals_ShouldReturnEmptyList()
        {
            // act
            var response = await _httpClient.GetAsync("api/goals");

            // assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<List<GetUserGoalDto>>();
            data.Should().HaveCount(0);
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnNewGoalDate()
        {
            // arrange
            var goal = await CreateGoalAsync();

            CreateGoalDateDto dto = CreateGoalDateDtoFaker
                .Generate(goal.Template.States[0].Id, goal.DateStart, goal.DaysNumber);

            // act
            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);
            
            // assert
            response.EnsureSuccessStatusCode();
            
            var data = await response.Content.ReadFromJsonAsync<GetGoalDateDto>();
            data.Should().NotBeNull();
            
            data!.Date.Should().Be(dto.Date);
            data.Comment.Should().Be(dto.Comment);
            data.StateId.Should().Be(dto.StateId);
        }

        private async Task<GetUserGoalDto> CreateGoalAsync()
        {
            CreateGoalDto goalDto = CreateGoalDtoFaker.Generate(_template.Id);
            var response = await _httpClient.PostAsJsonAsync("api/goals", goalDto);
            return (await response.Content.ReadFromJsonAsync<GetUserGoalDto>())!;
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnBad_WhenGoalDoesNotExist()
        {
            // arrange
            CreateGoalDateDto dto = CreateGoalDateDtoFaker
                .Generate(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), 12);

            // act
            var response = await _httpClient.PostAsJsonAsync($"api/goals/{Guid.NewGuid()}/dates", dto);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain(nameof(GoalItem));
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnBad_WhenStateIsWrong()
        {
            // arrange
            var goal = await CreateGoalAsync();

            CreateGoalDateDto dto = CreateGoalDateDtoFaker
                .Generate(Guid.NewGuid(), goal.DateStart, goal.DaysNumber);

            // act
            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("status");
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnBad_WhenDateBeforeStart()
        {
            // arrange
            var goal = await CreateGoalAsync();

            CreateGoalDateDto dto = CreateGoalDateDtoFaker
                .Generate(goal.Template.States[0].Id, goal.DateStart, goal.DaysNumber);
            dto = dto with { Date = goal.DateStart.AddDays(-1) };

            // act
            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("date");
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnBad_WhenDateAfterEnd()
        {
            // arrange
            var goal = await CreateGoalAsync();

            CreateGoalDateDto dto = CreateGoalDateDtoFaker
                .Generate(goal.Template.States[0].Id, goal.DateStart, goal.DaysNumber);
            dto = dto with { Date = goal.DateStart.AddDays(goal.DaysNumber) };

            // act
            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("date");
        }

        [Fact]
        public async Task CreateGoalDate_ShouldReturnBad_WhenDateInFuture()
        {
            // arrange
            var goal = await CreateGoalAsync();

            CreateGoalDateDto dto = CreateGoalDateDtoFaker
                .Generate(goal.Template.States[0].Id, goal.DateStart, goal.DaysNumber);
            dto = dto with { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)) };

            // act
            var response = await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("date");
        }

        [Fact]
        public async Task GetGoalDates_ShouldReturnOne()
        {
            // arrange
            var goal = await CreateGoalAsync();

            CreateGoalDateDto dto = CreateGoalDateDtoFaker
                .Generate(goal.Template.States[0].Id, goal.DateStart, goal.DaysNumber);
            await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);
            var date = dto.Date.ToDateTime(TimeOnly.MinValue).ToString("yyyy/MM/dd");

            // act
            var response = await _httpClient.GetAsync($"api/goals/{goal.Id}/dates?DateOfMonth={date}");

            // assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<List<GetGoalDateDto>>();
            data.Should().NotBeNull();
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetGoalDates_ShouldReturnEmpty()
        {
            // arrange
            var goal = await CreateGoalAsync();

            CreateGoalDateDto dto = CreateGoalDateDtoFaker
                .Generate(goal.Template.States[0].Id, goal.DateStart, goal.DaysNumber);
            await _httpClient.PostAsJsonAsync($"api/goals/{goal.Id}/dates", dto);
            var date = dto.Date.AddMonths(-1).ToDateTime(TimeOnly.MinValue).ToShortDateString();

            // act
            var response = await _httpClient.GetAsync($"api/goals/{goal.Id}/dates?DateOfMonth={date}");

            // assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<List<GetGoalDateDto>>();
            data.Should().NotBeNull();
            data.Should().BeEmpty();
        }
    }
}