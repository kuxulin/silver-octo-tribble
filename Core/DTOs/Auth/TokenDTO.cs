namespace Core.DTOs.Auth;
public class TokenDTO
{
    public string Token { get; set; } = null!;
    public string[]? Roles { get; set; }
    public string UserName { get; set; } = null!;
    public int Id { get; set; }
    public bool IsBlocked { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? ManagerId { get; set; }
}
