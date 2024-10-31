using Core.DTOs.Base;
using Core.DTOs.TodoTask;

namespace Core.DTOs.Change;
public class ChangeCreateDTO:BaseCreateDTO
{
    public int CreatorId { get; set; }
    public Guid? TaskId { get; set; }
    public string ActionType { get; set; } = null!;
    public string? TaskTitle { get; set; }
    public Guid ProjectId { get; set; }
}
