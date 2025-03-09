namespace TodoAppBackend.Models
{
    public class SubTask : BaseTask
    {
        public int? ParentSubTaskId { get; set; }
        public SubTask? ParentSubTask { get; set; }
        public List<SubTask> ChildSubTasks { get; set; } = new List<SubTask>();

        public int TodoItemId { get; set; }
        public TodoItem? TodoItem { get; set; }

        public List<UserSubTask> AssignedUsers { get; set; } = new List<UserSubTask>();

        // 🔹 Clone() - Tạo bản sao của SubTask
        public override BaseTask Clone()
        {
            var clone = (SubTask)base.Clone();
            clone.ParentSubTaskId = null;
            clone.ParentSubTask = null;
            clone.ChildSubTasks = ChildSubTasks.Select(st => (SubTask)st.Clone()).ToList();
            clone.AssignedUsers = AssignedUsers.Select(u => new UserSubTask { UserId = u.UserId, SubTaskId = u.SubTaskId }).ToList();
            return clone;
        }
        public override bool IsEqual(BaseTask other)
        {
            if (other is not SubTask otherSubTask) return false;

            bool baseEqual = base.IsEqual(otherSubTask);
            bool childTasksEqual = ChildSubTasks.Count == otherSubTask.ChildSubTasks.Count &&
                                   ChildSubTasks.All(st => otherSubTask.ChildSubTasks.Any(o => o.IsEqual(st)));
            bool usersEqual = AssignedUsers.Count == otherSubTask.AssignedUsers.Count &&
                              AssignedUsers.All(u => otherSubTask.AssignedUsers.Any(o => o.UserId == u.UserId));

            return baseEqual && childTasksEqual && usersEqual;
        }

        // 🔹 Tính phần trăm hoàn thành của SubTask dựa trên ChildSubTasks
        public double CompletionPercentage
        {
            get
            {
                if (ChildSubTasks == null || ChildSubTasks.Count == 0)
                    return IsCompleted ? 100.0 : 0.0; // Nếu không có con, chỉ dựa vào IsCompleted

                double completed = ChildSubTasks.Count(st => st.IsCompleted);
                return (completed / ChildSubTasks.Count) * 100.0;
            }
        }
    }
}
