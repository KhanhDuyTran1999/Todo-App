using TodoAppBackend.Models;
using TodoAppBackend.UnitOfWork;

namespace TodoAppBackend.Services
{
    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TodoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _unitOfWork.Todos.GetAllAsync();
        }

        public async Task<TodoItem> GetByIdAsync(int id)
        {
            return await _unitOfWork.Todos.GetByIdAsync(id);
        }

        public async Task<TodoItem> CreateAsync(string title)
        {
            var newTodo = new TodoItem { Title = title, IsCompleted = false };
            await _unitOfWork.Todos.AddAsync(newTodo);
            await _unitOfWork.SaveAsync();
            return newTodo;
        }

        public async Task<bool> UpdateAsync(TodoItem todo)
        {
            var existingTodo = await _unitOfWork.Todos.GetByIdAsync(todo.Id);
            if (existingTodo == null) return false;

            existingTodo.Title = todo.Title;
            existingTodo.IsCompleted = todo.IsCompleted;

            _unitOfWork.Todos.Update(existingTodo);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _unitOfWork.Todos.GetByIdAsync(id);
            if (todo == null) return false;

            _unitOfWork.Todos.Delete(todo);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<TodoItem>> GetTodosByUserIdAsync(int userId)
        {
            return await _unitOfWork.Todos.GetTodosByUserIdAsync(userId);
        }
    }

}
