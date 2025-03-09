using TodoAppBackend.Models;

namespace TodoAppBackend.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem> GetByIdAsync(int id);
        Task<TodoItem> CreateAsync(string title);
        Task<bool> UpdateAsync(TodoItem todo);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TodoItem>> GetTodosByUserIdAsync(int userId);
    }
    public interface ISubTaskService
    {
        Task<IEnumerable<SubTask>> GetAllAsync();
        Task<SubTask> GetByIdAsync(int id);
        Task<SubTask> CreateAsync(int todoItemId, string title);
        Task<bool> UpdateAsync(SubTask subTask);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<SubTask>> GetSubTasksByParentIdAsync(int parentId);
    }
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<User> GetUserWithTodosAsync(int userId);
    }

}
