using TodoAppBackend.Models;
using TodoAppBackend.UnitOfWork;

namespace TodoAppBackend.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubTaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SubTask>> GetAllAsync()
        {
            return await _unitOfWork.SubTasks.GetAllAsync();
        }

        public async Task<SubTask> GetByIdAsync(int id)
        {
            return await _unitOfWork.SubTasks.GetByIdAsync(id);
        }

        public async Task<SubTask> CreateAsync(int todoItemId, string title)
        {
            var newSubTask = new SubTask { Title = title, IsCompleted = false, TodoItemId = todoItemId };
            await _unitOfWork.SubTasks.AddAsync(newSubTask);
            await _unitOfWork.SaveAsync();
            return newSubTask;
        }

        public async Task<bool> UpdateAsync(SubTask subTask)
        {
            var existingSubTask = await _unitOfWork.SubTasks.GetByIdAsync(subTask.Id);
            if (existingSubTask == null) return false;

            existingSubTask.Title = subTask.Title;
            existingSubTask.IsCompleted = subTask.IsCompleted;

            _unitOfWork.SubTasks.Update(existingSubTask);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subTask = await _unitOfWork.SubTasks.GetByIdAsync(id);
            if (subTask == null) return false;
            var result = await _unitOfWork.SubTasks.Delete(id); // ✅ result là bool
            if (result)
            {
                Console.WriteLine("Xóa thành công!");
            }
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<SubTask>> GetSubTasksByParentIdAsync(int parentId)
        {
            return await _unitOfWork.SubTasks.GetSubTasksByParentIdAsync(parentId);
        }
    }

}
