namespace Core.Entities;
public class ApplicationImage :BaseEntity
{
    public string Name { get; set; } = null!;
    public byte[]?  Content { get; set; }
    public string Type { get; set; } = null!;
    public int? UserId { get; set; }
    public User User { get; set; } = null!;
}
