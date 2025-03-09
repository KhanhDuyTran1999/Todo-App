namespace TodoAppBackend.Models
{
    public abstract class BaseTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; } // Hạn chót hoàn thành

        // 🔹 Clone() - Tạo một bản sao của Task
        public virtual BaseTask Clone()
        {
            return (BaseTask)this.MemberwiseClone(); // 🔄 Tạo bản sao shallow copy
        }

        // 🔹 IsEqual() - Kiểm tra hai Task có giống nhau không
        public virtual bool IsEqual(BaseTask other)
        {
            if (other == null) return false;
            return Id == other.Id &&
                   Title == other.Title &&
                   Description == other.Description &&
                   IsCompleted == other.IsCompleted &&
                   CreatedAt == other.CreatedAt &&
                   DueDate == other.DueDate;
        }
    }
}
