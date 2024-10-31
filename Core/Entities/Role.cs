using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public partial class Role : IdentityRole<int>
{
    public ICollection<UserRole> UserRoles { get; set; } = null!;
}
