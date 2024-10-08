﻿using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public partial class User : IdentityUser<int>
{
    public DateTime CreationDate { get; set; }
    public string? RefreshToken { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsBlocked { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } 

    public User()
    {
        CreationDate = DateTime.UtcNow;
    }
}
