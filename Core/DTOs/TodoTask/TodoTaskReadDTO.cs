using Core.DTOs.Base;
using Core.DTOs.Employee;
using Core.Enums;

namespace Core.DTOs.TodoTask;
public class TodoTaskReadDTO :BaseReadDTO
{
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public EmployeeReadDTO? Employee { get; set; }
    public AvailableTaskStatus Status { get; set; }
}
