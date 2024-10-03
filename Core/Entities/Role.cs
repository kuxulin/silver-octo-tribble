using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public partial class Role : IdentityRole<Guid>
{
    public ICollection<UserRole> UserRoles { get; set; } 

}
