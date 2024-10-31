using System.ComponentModel.DataAnnotations;
namespace Core.DTOs.Auth;
public class LoginDTO
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
