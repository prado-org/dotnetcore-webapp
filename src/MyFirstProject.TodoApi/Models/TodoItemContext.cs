using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using MyFirstProject.TodoApi.Models;

namespace MyFirstProject.TodoApi.Models
{
    public class TodoItemContext : DbContext
    {
        public TodoItemContext(DbContextOptions<TodoItemContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TodoItem>()
            //    .HasOne(t => t.Category)
            //    .WithMany(c=> c.TodoItems)
            //    .HasForeignKey(t => t.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}