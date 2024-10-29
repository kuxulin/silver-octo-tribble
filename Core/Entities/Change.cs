namespace Core.Entities;
public class Change : BaseEntity
{
    public int? CreatorId { get; set; }
    public User? Creator { get; set; }
    public Guid? TaskId { get; set; }
    public TodoTask? Task { get; set; }
    public string ActionType { get; set; }
    public string? TaskTitle { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public ICollection<UserChange>? UserChanges { get; set; }
}
