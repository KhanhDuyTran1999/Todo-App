using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TodoAppBackend.Models;

namespace TodoAppBackend.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTodoItem> UserTodoItems { get; set; }
        public DbSet<UserSubTask> UserSubTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🟢 Thiết lập quan hệ giữa TodoItem và SubTask
            modelBuilder.Entity<TodoItem>()
                .HasMany(t => t.SubTasks)
                .WithOne(s => s.TodoItem)
                .HasForeignKey(s => s.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade); // ✅ Khi xóa TodoItem, tất cả SubTask cũng bị xóa

            // 🟢 Thiết lập quan hệ cha - con của SubTask (ParentSubTaskId)
            modelBuilder.Entity<SubTask>()
                .HasMany(s => s.ChildSubTasks)
                .WithOne(s => s.ParentSubTask)
                .HasForeignKey(s => s.ParentSubTaskId)
                .OnDelete(DeleteBehavior.Cascade); // ✅ Khi xóa một SubTask, các SubTask con cũng bị xóa

            // 🟢 Thiết lập quan hệ User - TodoItem (Bảng trung gian)
            modelBuilder.Entity<UserTodoItem>()
                .HasKey(ut => new { ut.UserId, ut.TodoItemId });

            modelBuilder.Entity<UserTodoItem>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.TodoItems)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTodoItem>()
                .HasOne(ut => ut.TodoItem)
                .WithMany(t => t.AssignedUsers)
                .HasForeignKey(ut => ut.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🟢 Thiết lập quan hệ User - SubTask (Bảng trung gian)
            modelBuilder.Entity<UserSubTask>()
                .HasKey(us => new { us.UserId, us.SubTaskId });

            modelBuilder.Entity<UserSubTask>()
                .HasOne(us => us.User)
                .WithMany(u => u.SubTasks)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSubTask>()
                .HasOne(us => us.SubTask)
                .WithMany(st => st.AssignedUsers)
                .HasForeignKey(us => us.SubTaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
