using TodoAppBackend.Models;
using TodoAppBackend.UnitOfWork;

namespace TodoAppBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var existingUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
            if (existingUser == null) return false;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            _unitOfWork.Users.Update(existingUser);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return false;

            var result = await _unitOfWork.Users.Delete(id); // ✅ result là bool
            if (result)
            {
                Console.WriteLine("Xóa thành công!");
            }
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<User> GetUserWithTodosAsync(int userId)
        {
            return await _unitOfWork.Users.GetUserWithTodosAsync(userId);
        }

    }

}
