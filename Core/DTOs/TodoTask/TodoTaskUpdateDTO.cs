using Core.DTOs.Base;
using Core.Enums;

namespace Core.DTOs.TodoTask;
public class TodoTaskUpdateDTO :BaseUpdateDTO
{
    public string? Title { get; set; }
    public string? Text { get; set; }

    public Guid? EmployeeId { get; set; }
    public AvailableTaskStatus? Status { get; set; }
}
