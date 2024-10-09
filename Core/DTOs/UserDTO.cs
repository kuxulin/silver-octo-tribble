using Core.Enums;

namespace Core.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public AvailableUserRole[] Roles { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsBlocked { get; set; }
}
