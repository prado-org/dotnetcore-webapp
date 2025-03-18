namespace MyFirstProject.TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? DueDate { get; set; }

        // New property for category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}