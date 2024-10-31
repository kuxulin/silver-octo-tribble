using Core.DTOs.Base;

namespace Core.DTOs.TodoTask;
public class TodoTaskCreateDTO : BaseCreateDTO
{
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public Guid ProjectId { get; set; } 
    public Guid? EmployeeId { get; set; }
}
