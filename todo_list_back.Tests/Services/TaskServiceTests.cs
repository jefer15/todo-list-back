using Xunit;
using Microsoft.EntityFrameworkCore;
using todo_list_back.Data;
using todo_list_back.Models;
using todo_list_back.Services;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace todo_list_back.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly AppDbContext _context;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // BD única por test
                .Options;

            _context = new AppDbContext(options);
            _service = new TaskService(_context);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddTask()
        {
            var task = new TaskItem { Title = "Tarea 1", Description = "Test task" };
            int userId = 1;

            var created = await _service.CreateAsync(userId, task);
            var tasks = await _service.GetTasksAsync();

            Assert.NotNull(created);
            Assert.Single(tasks);
            Assert.Equal("Tarea 1", tasks.First().Title);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyTask()
        {
            var task = new TaskItem { Title = "Tarea inicial", UserId = 1 };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            task.Title = "Tarea actualizada";

            var updated = await _service.UpdateAsync(task.Id, task);

            Assert.NotNull(updated);
            Assert.Equal("Tarea actualizada", updated.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTask()
        {
            var task = new TaskItem { Title = "Para eliminar", UserId = 1 };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAsync(task.Id);
            var remaining = await _context.Tasks.ToListAsync();

            Assert.True(result);
            Assert.Empty(remaining);
        }
    }
}
