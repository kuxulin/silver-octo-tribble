namespace Core.Entities;
public class Employee :BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<Project> Projects { get; set; }
}
