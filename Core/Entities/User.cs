using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public partial class User : IdentityUser<Guid>
{
    public DateTime CreationDate { get; set; }
    public string? RefreshToken { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsBlocked { get; set; }
    public User()
    {
        CreationDate = DateTime.UtcNow;
    }
}
