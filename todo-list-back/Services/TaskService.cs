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

        public async Task<IEnumerable<TaskItem>> GetTasksAsync(string? status = null)
        {
            var query = _context.Tasks
                .Include(t => t.User)
                .AsQueryable();

            if (status == "completed")
                query = query.Where(t => t.IsCompleted);
            else if (status == "pending")
                query = query.Where(t => !t.IsCompleted);

            return await query.OrderByDescending(t => t.Id).ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int taskId)
        {
            return await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<TaskItem> CreateAsync(int userId, TaskItem item)
        {
            item.UserId = userId;
            item.CreatedAt = DateTime.UtcNow;

            _context.Tasks.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TaskItem?> UpdateAsync(int taskId, TaskItem item)
        {
            var existing = await _context.Tasks.FindAsync(taskId);
            if (existing == null) return null;

            existing.Title = item.Title;
            existing.Description = item.Description;
            existing.IsCompleted = item.IsCompleted;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<TaskItem?> UpdateStatusAsync(int taskId, bool status)
        {
            var existing = await _context.Tasks.FindAsync(taskId);
            if (existing == null) return null;

            existing.IsCompleted = status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int taskId)
        {
            var existing = await _context.Tasks.FindAsync(taskId);
            if (existing == null) return false;

            _context.Tasks.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetSummaryAsync()
        {
            var total = await _context.Tasks.CountAsync();
            var completed = await _context.Tasks.CountAsync(t => t.IsCompleted);
            var pending = total - completed;

            return new
            {
                total,
                completed,
                pending
            };
        }
    }
}
