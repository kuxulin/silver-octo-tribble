using Core.DTOs.Base;
using Core.DTOs.TodoTask;
using Core.DTOs.User;

namespace Core.DTOs.Change;
public class ChangeReadDTO : BaseReadDTO
{
    public UserReadDTO Creator { get; set; }
    public TodoTaskReadDTO Task { get; set; }
    public string ActionType { get; set; }
    public string? TaskTitle { get; set; }
    public Guid ProjectId { get; set; }
}
