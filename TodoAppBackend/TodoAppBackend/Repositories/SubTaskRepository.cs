using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoAppBackend.Data;
using TodoAppBackend.Models;

namespace TodoAppBackend.Repositories
{
    public class SubTaskRepository : ISubTaskRepository
    {
        private readonly TodoDbContext _context;

        public SubTaskRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubTask>> GetAllAsync() => await _context.SubTasks.ToListAsync();

        public async Task<SubTask> GetByIdAsync(int id) => await _context.SubTasks.FindAsync(id) ?? throw new KeyNotFoundException($"User với ID {id} không tồn tại.");

        public async Task<IEnumerable<SubTask>> GetSubTasksByParentIdAsync(int parentId) =>
            await _context.SubTasks.Where(st => st.ParentSubTaskId == parentId).ToListAsync();

        public async Task<IEnumerable<SubTask>> FindAsync(Expression<Func<SubTask, bool>> predicate) =>
            await _context.SubTasks.Where(predicate).ToListAsync();

        public async Task AddAsync(SubTask entity) => await _context.SubTasks.AddAsync(entity);

        public void Update(SubTask entity) => _context.SubTasks.Update(entity);

        public async Task<bool> CanDeleteAsync(int subTaskId)
        {
            return !await _context.SubTasks.AnyAsync(st => st.ParentSubTaskId == subTaskId);
        }

        public async Task<bool> Delete(int id)
        {
            if (!await CanDeleteAsync(id))
            {
                throw new InvalidOperationException("Không thể xóa SubTask vì có SubTask con liên quan.");
            }

            var subTask = await _context.SubTasks.FindAsync(id);
            if (subTask == null) throw new KeyNotFoundException($"SubTask với ID {id} không tồn tại.");

            _context.SubTasks.Remove(subTask);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
