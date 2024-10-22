using Core.DTOs.Base;
using Core.DTOs.Employee;
using Core.Enums;

namespace Core.DTOs.TodoTask;
public class TodoTaskReadDTO :BaseReadDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public Guid ProjectId { get; set; }
    public EmployeeReadDTO? Employee { get; set; }
    public AvailableTaskStatus Status { get; set; }
}
