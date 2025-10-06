using System.Net;
using System.Net.Http.Json;
using todo_list_back.DTOs;
using todo_list_back.Tests.Utils;
using Xunit;
using System;

namespace todo_list_back.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturn200_WhenUserIsNew()
        {
            var dto = new RegisterDto
            {
                Name = "UserTest",
                Email = $"user_{Guid.NewGuid()}@test.com", // 👈 evita duplicados
                Password = "123456"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/register", dto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreCorrect()
        {
            var email = $"login_{Guid.NewGuid()}@test.com";

            var register = new RegisterDto
            {
                Name = "UserTest2",
                Email = email,
                Password = "123456"
            };

            await _client.PostAsJsonAsync("/api/auth/register", register);

            var login = new LoginDto
            {
                Email = email,
                Password = "123456"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/login", login);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            Assert.False(string.IsNullOrEmpty(result.Token));
        }
    }
}
