using Microsoft.EntityFrameworkCore;
using todo_list_back.Data;
using todo_list_back.Models;

namespace todo_list_back.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int userId, int taskId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        }

        public async Task<TaskItem> CreateAsync(int userId, TaskItem item)
        {
            item.UserId = userId;
            item.CreatedAt = DateTime.UtcNow;

            _context.Tasks.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TaskItem?> UpdateAsync(int userId, int taskId, TaskItem item)
        {
            var existing = await GetByIdAsync(userId, taskId);
            if (existing == null) return null;

            existing.Title = item.Title;
            existing.Description = item.Description;
            existing.IsCompleted = item.IsCompleted;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int userId, int taskId)
        {
            var existing = await GetByIdAsync(userId, taskId);
            if (existing == null) return false;

            _context.Tasks.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
