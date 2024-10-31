namespace Core.Entities;
public class Manager :BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Project>? Projects { get; set; }
}
