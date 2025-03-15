using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoAppBackend.Data;
using TodoAppBackend.Models;

namespace TodoAppBackend.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync() => await _context.TodoItems.Include(t => t.SubTasks).ToListAsync();

        public async Task<TodoItem> GetByIdAsync(int id) =>
            await _context.TodoItems.Include(t => t.SubTasks).FirstOrDefaultAsync(t => t.Id == id) ?? throw new KeyNotFoundException($"User với ID {id} không tồn tại.");

        public async Task<IEnumerable<TodoItem>> GetTodosByUserIdAsync(int userId) =>
            await _context.TodoItems
                         .Where(t => t.AssignedUsers.Any(ut => ut.UserId == userId))
                         .Include(t => t.SubTasks)
                         .ToListAsync();

        public async Task<IEnumerable<TodoItem>> FindAsync(Expression<Func<TodoItem, bool>> predicate) =>
            await _context.TodoItems.Include(t => t.SubTasks).Where(predicate).ToListAsync();

        public async Task AddAsync(TodoItem entity) => await _context.TodoItems.AddAsync(entity);

        public void Update(TodoItem entity) => _context.TodoItems.Update(entity);

        public async Task<bool> CanDeleteAsync(int taskId)
        {
            return !await _context.SubTasks.AnyAsync(st => st.TodoItemId == taskId);
        }

        public async Task<bool> Delete(int id)
        {
            if (!await CanDeleteAsync(id))
            {
                throw new InvalidOperationException("Không thể xóa Task vì có SubTask liên quan.");
            }

            var task = await _context.TodoItems.FindAsync(id);
            if (task == null) throw new KeyNotFoundException($"Task với ID {id} không tồn tại.");

            _context.TodoItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
