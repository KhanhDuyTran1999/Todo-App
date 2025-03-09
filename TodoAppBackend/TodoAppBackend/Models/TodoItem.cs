namespace TodoAppBackend.Models
{
    public class TodoItem : BaseTask
    {
        public int CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public List<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public List<UserTodoItem> AssignedUsers { get; set; } = new List<UserTodoItem>();

        // 🔹 Clone() - Tạo bản sao của TodoItem
        public override BaseTask Clone()
        {
            var clone = (TodoItem)base.Clone();
            clone.SubTasks = SubTasks.Select(st => (SubTask)st.Clone()).ToList();
            clone.AssignedUsers = AssignedUsers.Select(u => new UserTodoItem { UserId = u.UserId, TodoItemId = u.TodoItemId }).ToList();
            return clone;
        }

        // 🔹 IsEqual() - So sánh TodoItem với một BaseTask khác
        public override bool IsEqual(BaseTask other)
        {
            if (other is not TodoItem otherTodo) return false;

            bool baseEqual = base.IsEqual(otherTodo);
            bool subTasksEqual = SubTasks.Count == otherTodo.SubTasks.Count &&
                                 SubTasks.All(st => otherTodo.SubTasks.Any(o => o.IsEqual(st)));
            bool usersEqual = AssignedUsers.Count == otherTodo.AssignedUsers.Count &&
                              AssignedUsers.All(u => otherTodo.AssignedUsers.Any(o => o.UserId == u.UserId));

            return baseEqual && subTasksEqual && usersEqual;
        }

        // 🔹 Tính phần trăm hoàn thành dựa trên các SubTask
        public double CompletionPercentage
        {
            get
            {
                if (SubTasks == null || SubTasks.Count == 0)
                    return IsCompleted ? 100.0 : 0.0;

                double completed = SubTasks.Count(st => st.IsCompleted);
                return (completed / SubTasks.Count) * 100.0;
            }
        }
    }
}
