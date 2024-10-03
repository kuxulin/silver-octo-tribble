using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Auth;

public class RegisterDTO
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    [RegularExpression(@"^\+\d{8,15}$", ErrorMessage = "Entered phone format is not valid.")]
    public string? PhoneNumber { get; set; }
}

