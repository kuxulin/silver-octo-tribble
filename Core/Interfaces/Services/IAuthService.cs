using Core.DTOs.Auth;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface IAuthService
{
    Task<Result<TokenDTO>> Register(RegisterDTO dto);
    Task<Result<TokenDTO>> Login(LoginDTO dto);
    Task<Result<TokenDTO>> CreateAccessTokenFromRefresh(string oldRefreshToken);
    Task<Result<string>> CreateRefreshTokenAsync(string username);
}
