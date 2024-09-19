using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public partial class User : IdentityUser<Guid>
{
    public DateTime CreationDate { get; set; }
}
