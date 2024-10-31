namespace Core.Entities;
public class Employee :BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Project>? Projects { get; set; }
    public ICollection<TodoTask>? TodoTasks { get; set; }
}
