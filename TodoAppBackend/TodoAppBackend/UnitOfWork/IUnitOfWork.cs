using TodoAppBackend.Repositories;

namespace TodoAppBackend.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoRepository Todos { get; }
        ISubTaskRepository SubTasks { get; }
        IUserRepository Users { get; }

        Task<int> SaveAsync(); // ✅ Lưu tất cả thay đổi vào Database
    }
}
