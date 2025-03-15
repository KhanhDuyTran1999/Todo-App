using System.Linq.Expressions;
using TodoAppBackend.Models;

namespace TodoAppBackend.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        Task<bool> Delete(int id);
        Task<bool> CanDeleteAsync(int id);
    }
    public interface ITodoRepository : IRepository<TodoItem>
    {
        Task<IEnumerable<TodoItem>> GetTodosByUserIdAsync(int userId);
    }
    public interface ISubTaskRepository : IRepository<SubTask>
    {
        Task<IEnumerable<SubTask>> GetSubTasksByParentIdAsync(int parentId);
    }
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserWithTodosAsync(int userId);
    }
}
