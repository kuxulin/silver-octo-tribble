using Core.DTOs.ApplicationImage;
using Core.Enums;

namespace Core.DTOs.User;

public class UserReadDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public IEnumerable<int> RoleIds { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsBlocked { get; set; }
    public ApplicationImageReadDTO Image { get; set; }
}
