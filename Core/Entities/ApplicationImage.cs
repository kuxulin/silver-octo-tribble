namespace Core.Entities;
public class ApplicationImage :BaseEntity
{
    public string Name { get; set; }
    public  string  Content { get; set; }   
    public int UserId { get; set; }
}
