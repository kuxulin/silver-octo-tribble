namespace Core.Entities;
public class Employee :BaseEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public ICollection<Project> Projects { get; set; }
}
