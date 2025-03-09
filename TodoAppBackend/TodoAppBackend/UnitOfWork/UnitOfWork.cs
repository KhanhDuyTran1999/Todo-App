using TodoAppBackend.Data;
using TodoAppBackend.Repositories;

namespace TodoAppBackend.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoDbContext _context;

        public ITodoRepository Todos { get; }
        public ISubTaskRepository SubTasks { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(TodoDbContext context, ITodoRepository todoRepository, ISubTaskRepository subTaskRepository, IUserRepository userRepository)
        {
            _context = context;
            Todos = todoRepository;
            SubTasks = subTaskRepository;
            Users = userRepository;
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
