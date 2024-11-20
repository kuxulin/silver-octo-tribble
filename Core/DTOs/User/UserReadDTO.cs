using Core.DTOs.ApplicationImage;

namespace Core.DTOs.User;

public class UserReadDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public IEnumerable<int>? RoleIds { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsBlocked { get; set; }
    public ImageReadDTO Image { get; set; } = null!;
    public Guid ImageId { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid? EmployeeId { get; set; }
}
