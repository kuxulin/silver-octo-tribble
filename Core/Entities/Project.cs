namespace Core.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description {  get; set; } = null!;
    public ICollection<TodoTask>? ToDoTasks { get; set; }

    public ICollection<Manager> Managers { get; set; } = null!;
    public ICollection<Employee>? Employees { get; set; }
}
