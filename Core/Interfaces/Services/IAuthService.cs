using Core.DTOs.Auth;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface IAuthService
{
    Task<Result<TokenDTO>> Register(AuthDTO dto);
    Task<Result<TokenDTO>> Login(AuthDTO dto);
    Task<Result<TokenDTO>> CreateAccessTokenFromRefresh(string oldRefreshToken);
    Task<Result<string>> CreateRefreshTokenAsync(string username);
}
