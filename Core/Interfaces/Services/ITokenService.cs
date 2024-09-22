using Core.Entities;
using System.Security.Claims;

namespace Core.Interfaces.Services;
public interface ITokenService
{
    Task<string> CreateAccessTokenAsync(User user, IEnumerable<string> userRoles);
    string CreateRefreshToken(string username);
    ClaimsPrincipal GetTokenPrincipal(string token);
}
