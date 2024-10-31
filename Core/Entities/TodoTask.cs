namespace Core.Entities;

public class TodoTask : BaseEntity
{ 
    public string Title { get; set; } = null!;

    public string Text { get; set; } = null!;

    public Guid ProjectId { get; set; }

    public Project Project { get; set; } = null!;
    public int StatusId { get; set; }
    public TodoTaskStatus Status { get; set; } = null!;
    public Guid? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
