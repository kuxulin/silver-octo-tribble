using Core.DTOs.Auth;

namespace Core.Interfaces.Services;
public interface IAuthService
{
    Task<TokenDTO> Register(AuthDTO dto);
    Task<TokenDTO> Login(AuthDTO dto);
    Task<TokenDTO> RefreshTokens(string oldRefreshToken);
}
