namespace TodoAppBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";

        public List<UserTodoItem> TodoItems { get; set; } = new List<UserTodoItem>(); // Công việc chính
        public List<UserSubTask> SubTasks { get; set; } = new List<UserSubTask>(); // Công việc con

        // Phương thức so sánh
        public bool IsEqual(User other)
        {
            if (other == null) return false;

            return Id == other.Id &&
                   Name == other.Name &&
                   Email == other.Email;
        }

        // 🔹 Clone() - Tạo bản sao của User
        public User Clone()
        {
            return new User
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                TodoItems = this.TodoItems.Select(t => new UserTodoItem { UserId = t.UserId, TodoItemId = t.TodoItemId }).ToList(),
                SubTasks = this.SubTasks.Select(t => new UserSubTask { UserId = t.UserId, SubTaskId = t.SubTaskId }).ToList()
            };
        }
    }
}
