using Core.DTOs.Base;
using Core.DTOs.TodoTask;
using Core.DTOs.User;

namespace Core.DTOs.Change;
public class ChangeReadDTO : BaseReadDTO
{
    public required UserReadDTO Creator { get; set; }
    public required TodoTaskReadDTO Task { get; set; }
    public string ActionType { get; set; } = null!;
    public string? TaskTitle { get; set; }
    public Guid ProjectId { get; set; }
    public bool IsRead { get; set; }
}
