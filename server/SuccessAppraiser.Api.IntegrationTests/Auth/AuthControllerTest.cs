using FluentAssertions;
using SuccessAppraiser.Api.Auth.Contracts;
using SuccessAppraiser.Api.IntegrationTests.Common;
using SuccessAppraiser.Api.IntegrationTests.Fakers;
using System.Net;
using System.Net.Http.Json;

namespace SuccessAppraiser.Api.IntegrationTests.Auth
{
    public class AuthControllerTest : BaseIntegrationTest, IDisposable
    {
        public AuthControllerTest(ApiWebApplicationFactory webFactory) : base(webFactory)
        {
        }

        public void Dispose()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _httpClient.DefaultRequestHeaders.Remove("Cookie");
        }

        [Fact]
        public async Task Register_ShouldBeOk()
        {
            // arrange
            RegisterDto dto = RegisterDtoFaker.Generate();

            // act
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Register_ShouldBeConflict_WhenUserAlreadyExists()
        {
            // arrange
            RegisterDto dto = RegisterDtoFaker.Generate();

            // act
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);
            var secondResponse = await _httpClient.PostAsJsonAsync("api/auth/register", dto);

            // assert
            response.EnsureSuccessStatusCode();
            secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Login_ShouldBeOk()
        {
            // arrange
            RegisterDto registerDto = RegisterDtoFaker.Generate();
            await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);

            LoginDto dto = new LoginDto(registerDto.Email, registerDto.Password);

            // act
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            // assert
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadFromJsonAsync<AuthDto>();
            responseData.Should().NotBeNull();

            responseData!.AccessToken.Should().NotBeNullOrEmpty();
            responseData.Username.Should().NotBeNullOrEmpty();
            responseData.Username.Should().Be(registerDto.Username);

            string cookie = response.Headers.GetValues("Set-Cookie")
                .FirstOrDefault(x => x.Contains("X-Refresh-Token"))!;

            cookie.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_ShouldBeUnauthorized_WhenCredentialsWrong()
        {
            // arrange
            LoginDto dto = new LoginDto("wrong@mail.ru", "Password123");

            // act
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Refresh_ShouldBeUnauthorized_WhenNoCookie()
        {
            // act
            var response = await _httpClient.GetAsync("api/auth/refresh");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Refresh_ShouldBeUnauthorized_WhenTokenInvalid()
        {
            // arrange
            _httpClient.DefaultRequestHeaders.Add("Cookie", "X-Refresh-Token=invalid cookie");

            // act
            var refreshResponse = await _httpClient.GetAsync("api/auth/refresh");

            // assert
            refreshResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Refresh_ShouldBeOk()
        {
            // arrange
            RegisterDto registerDto = RegisterDtoFaker.Generate();
            await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);

            LoginDto dto = new LoginDto(registerDto.Email, registerDto.Password);
            var loginResponse = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            string cookie = loginResponse.Headers.GetValues("Set-Cookie")
                .FirstOrDefault(x => x.Contains("X-Refresh-Token"))!;

            _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);

            // act
            var refreshResponse = await _httpClient.GetAsync("api/auth/refresh");

            // assert
            refreshResponse.EnsureSuccessStatusCode();

            var responseData = await refreshResponse.Content.ReadFromJsonAsync<AuthDto>();
            responseData.Should().NotBeNull();

            responseData!.AccessToken.Should().NotBeNullOrEmpty();
            responseData.Username.Should().NotBeNullOrEmpty();

            string newCookie = refreshResponse.Headers.GetValues("Set-Cookie")
                .FirstOrDefault(x => x.Contains("X-Refresh-Token"))!;

            newCookie.Should().NotBeNullOrEmpty();
        }
    }
}