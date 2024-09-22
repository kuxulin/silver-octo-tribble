using Core.Constants;
using Core.DTOs.Auth;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<TokenDTO> Login(AuthDTO dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);

        if (user is null)
            throw new Exception("There is no such user in db");

        if (await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return await GenerateTokensAsync(user);
        }

        throw new Exception("Passwords aren`t equal");
    }

    public async Task<TokenDTO> Register(AuthDTO dto)
    {
        User? possibleUser = await _userManager.FindByNameAsync(dto.UserName);

        if (possibleUser is not null)
            throw new Exception("There is already such username in db");

        User user = new()
        {
            Email = dto.Email,
            UserName = dto.UserName,
            CreationDate = DateTime.UtcNow,
        };

        IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
        return await GenerateTokensAsync(user);
    }

    public async Task<TokenDTO> RefreshTokens(string oldRefreshToken)
    {
        var principal = _tokenService.GetTokenPrincipal(oldRefreshToken);

        if (principal?.Identity?.Name is null)
            throw new Exception("Invalid refresh token");

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);

        if (user is null)
            throw new Exception("There is no such user in db");

        return await GenerateTokensAsync(user);
    }

    private async Task<TokenDTO> GenerateTokensAsync(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var accessToken = await _tokenService.CreateAccessTokenAsync(user, userRoles);
        var refreshToken = _tokenService.CreateRefreshToken(user.UserName);

        return new TokenDTO
        {
            Token = accessToken,
            UserName = user.UserName,
            RefreshToken = refreshToken,
        };
    }
}
