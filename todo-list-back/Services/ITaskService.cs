using todo_list_back.Models;

namespace todo_list_back.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync(int userId);
        Task<TaskItem?> GetByIdAsync(int userId, int taskId);
        Task<TaskItem> CreateAsync(int userId, TaskItem item);
        Task<TaskItem?> UpdateAsync(int userId, int taskId, TaskItem item);
        Task<bool> DeleteAsync(int userId, int taskId);
    }
}
