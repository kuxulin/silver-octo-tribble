namespace Core.Entities;
public class UserChange
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid ChangeId { get; set; }
    public Change Change {  get; set; } = null!;
    public bool IsRead { get; set; }
}
