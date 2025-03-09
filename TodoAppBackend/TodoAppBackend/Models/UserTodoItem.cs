namespace TodoAppBackend.Models
{
    public class UserTodoItem
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int TodoItemId { get; set; }
        public TodoItem? TodoItem { get; set; }
    }
}
