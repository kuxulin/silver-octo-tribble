namespace Core.DTOs.Auth;
public class TokenDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string[]? Roles { get; set; }
    public string? UserName { get; set; }
}
