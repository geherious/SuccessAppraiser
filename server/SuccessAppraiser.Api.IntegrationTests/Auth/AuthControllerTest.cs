using FluentAssertions;
using SuccessAppraiser.Api.Auth.Contracts;
using SuccessAppraiser.Api.IntegrationTests.Common;
using SuccessAppraiser.Api.IntegrationTests.TestObjects;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SuccessAppraiser.Api.IntegrationTests.Auth
{
    public class AuthControllerTest : BaseIntegrationTest
    {
        public AuthControllerTest(ApiWebApplicationFactory webFactory) : base(webFactory)
        {
        }

        [Fact]
        public async Task Register_ShouldBeOk()
        {
            RegisterDto dto = new RegisterDto("testUserName", "test@gmail.com", "Password123");

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);

            response.EnsureSuccessStatusCode();
            _dbContext.ApplicationUsers.ToArray();
        }

        [Fact]
        public async Task Register_ShouldBeConflict_WhenUserAlreadyExists()
        {
            RegisterDto dto = new RegisterDto("repeatUsername", "repeatUsername@gmail.com", "Password123");

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);
            var secondResponse = await _httpClient.PostAsJsonAsync("api/auth/register", dto);

            response.EnsureSuccessStatusCode();
            secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Login_ShouldBeOk()
        {
            var user = AuthTestObjects.getBaseUser();
            LoginDto dto = new LoginDto(user.Email!, "Password123");

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<AuthDto>();
            responseData!.AccessToken.Should().NotBeNullOrEmpty();
            responseData.Username.Should().NotBeNullOrEmpty();
            response.Headers.Should().ContainKey("Set-Cookie").WhoseValue.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_ShouldBeUnauthorized_WhenCredentialsWrong()
        {
            LoginDto dto = new LoginDto("wrong@mail.ru", "Password123");

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Refresh_ShouldBeUnauthorized_WhenNoCookie()
        {

            var response = await _httpClient.GetAsync("api/auth/refresh");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Refresh_ShouldBeUnauthorized_WhenTokenInvalid()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_token");

            var refreshResponse = await _httpClient.GetAsync("api/auth/refresh");

            refreshResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        [Fact]
        public async Task Refresh_ShouldBeOk()
        {
            var user = AuthTestObjects.getBaseUser();
            LoginDto dto = new LoginDto(user.Email!, "Password123");
            var loginResponse = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            string cookie = loginResponse.Headers.GetValues("Set-Cookie")
                .FirstOrDefault(x => x.Contains("X-Refresh-Token"))!;
            _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            var refreshResponse = await _httpClient.GetAsync("api/auth/refresh");

            refreshResponse.EnsureSuccessStatusCode();
            var responseData = await refreshResponse.Content.ReadFromJsonAsync<AuthDto>();
            responseData!.AccessToken.Should().NotBeNullOrEmpty();
            responseData.Username.Should().NotBeNullOrEmpty();
            refreshResponse.Headers.Should().ContainKey("Set-Cookie").WhoseValue.Should().NotBeNullOrEmpty();

        }
    }
}