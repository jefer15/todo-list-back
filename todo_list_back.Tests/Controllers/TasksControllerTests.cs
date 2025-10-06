using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using todo_list_back.DTOs;
using todo_list_back.Models;
using todo_list_back.Tests.Utils;
using Xunit;
using System;
using System.Threading.Tasks;

namespace todo_list_back.Tests.Controllers
{
    public class TasksControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private string _token;

        public TasksControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task AuthenticateAsync()
        {
            var email = $"task_{Guid.NewGuid()}@test.com";

            var register = new RegisterDto
            {
                Name = "TaskUser",
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
            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            _token = result.Token;
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
        }

        [Fact]
        public async Task CreateTask_ShouldReturn201_WhenAuthorized()
        {
            await AuthenticateAsync();

            var task = new TaskItem
            {
                Title = "Nueva tarea",
                Description = "Creación de prueba"
            };

            var response = await _client.PostAsJsonAsync("/api/tasks", task);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetTasks_ShouldReturn200_WhenAuthorized()
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync("/api/tasks");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
