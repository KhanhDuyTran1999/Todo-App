namespace TodoAppBackend.Models
{
    public class UserSubTask
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int SubTaskId { get; set; }
        public SubTask? SubTask { get; set; }
    }
}
