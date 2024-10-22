namespace Core.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; }
    public string Description {  get; set; }
    public ICollection<TodoTask> ToDoTasks { get; set; }

    public ICollection<Manager> Managers { get; set; }
    public ICollection<Employee> Employees { get; set; }
}
