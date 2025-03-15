using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoAppBackend.Data;
using TodoAppBackend.Models;

namespace TodoAppBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoDbContext _context;

        public UserRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

        public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException($"User với ID {id} không tồn tại.");

        public async Task<User> GetUserWithTodosAsync(int userId) =>
            await _context.Users
                          .Include(u => u.TodoItems)
                          .ThenInclude(ut => ut.TodoItem)
                          .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException($"User với ID {userId} không tồn tại.");

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate) =>
            await _context.Users.Where(predicate).ToListAsync();

        public async Task AddAsync(User entity) => await _context.Users.AddAsync(entity);

        public void Update(User entity) => _context.Users.Update(entity);

        public void Delete(User entity) => _context.Users.Remove(entity);

        public async Task<bool> CanDeleteAsync(int userId)
        {
            return !await _context.UserTodoItems.AnyAsync(uti => uti.UserId == userId) &&
                   !await _context.UserSubTasks.AnyAsync(ust => ust.UserId == userId);
        }

        public async Task<bool> Delete(int id)
        {
            if (!await CanDeleteAsync(id))
            {
                throw new InvalidOperationException("Không thể xóa User vì User này đang có Task hoặc SubTask.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException($"User với ID {id} không tồn tại.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
