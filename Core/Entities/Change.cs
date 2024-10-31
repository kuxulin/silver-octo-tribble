namespace Core.Entities;
public class Change : BaseEntity
{
    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;
    public Guid? TaskId { get; set; }
    public TodoTask? Task { get; set; }
    public string ActionType { get; set; } = null!;
    public string TaskTitle { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public ICollection<UserChange>? UserChanges { get; set; }
}
