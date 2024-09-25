using Core.DTOs.Auth;

namespace Core.Interfaces.Services;
public interface IAuthService
{
    Task<TokenDTO> Register(AuthDTO dto);
    Task<TokenDTO> Login(AuthDTO dto);
    Task<TokenDTO> CreateAccessTokenFromRefresh(string oldRefreshToken);
    Task<string> CreateRefreshTokenAsync(string username);
}
