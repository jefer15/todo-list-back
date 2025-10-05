using todo_list_back.Models;

namespace todo_list_back.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync(string? status = null);
        Task<TaskItem?> GetByIdAsync(int taskId);
        Task<TaskItem> CreateAsync(int userId, TaskItem item);
        Task<TaskItem?> UpdateAsync(int taskId, TaskItem item);
        Task<TaskItem?> UpdateStatusAsync(int taskId, bool status);
        Task<bool> DeleteAsync(int taskId);
        Task<object> GetSummaryAsync();
    }

}
