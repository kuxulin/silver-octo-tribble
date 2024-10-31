using Microsoft.AspNetCore.Identity;

namespace Core.Entities;
public class UserRole : IdentityUserRole<int>
{
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
