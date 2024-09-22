namespace Core.Entities;
public class Manager :BaseEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public ICollection<Project> Projects { get; set; }
}
