namespace Core.Entities;
public class ApplicationImage :BaseEntity
{
    public string Name { get; set; } = null!;
    public  string  Content { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
