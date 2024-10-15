using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class User : IdentityUser<int>
{
    public DateTime CreationDate { get; set; }
    public string? RefreshToken { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsBlocked { get; set; }
    public Guid? ImageId { get; set; }
    public ApplicationImage? Image { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } 
    public Guid? ManagerId { get; set; }
    public Manager? Manager { get; set; }
    public Guid? EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public User()
    {
        CreationDate = DateTime.UtcNow;
    }
}
