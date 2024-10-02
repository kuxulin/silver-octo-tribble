using Core.Constants;
using Core.DTOs.Auth;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.AspNetCore.Identity;

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

    public async Task<Result<TokenDTO>> Login(LoginDTO dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);

        if (user is null)
            return DefinedError.AbsentElement; 

        if (await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return await GenerateAccessTokenAsync(user);
        }

        return DefinedError.NonEqualPasswords; 
    }

    public async Task<Result<TokenDTO>> Register(RegisterDTO dto)
    {
        User? possibleUser = await _userManager.FindByNameAsync(dto.UserName);

        if (possibleUser is not null)
            return DefinedError.DuplicateEntity;

        User user = new()
        {
            PhoneNumber = dto.PhoneNumber,
            UserName = dto.UserName,
            FullName = dto.FullName,
        };

        IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
        return await GenerateAccessTokenAsync(user);
    }

    public async Task<Result<TokenDTO>> CreateAccessTokenFromRefresh(string oldRefreshToken)
    {
        if(!_tokenService.ValidateToken(oldRefreshToken))
            return DefinedError.InvalidElement;

        string username = _tokenService.GetNameFromToken(oldRefreshToken);

        if (username is null)
            return DefinedError.InvalidElement;

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
            return DefinedError.AbsentElement; 

        if(user.RefreshToken != oldRefreshToken)
            return DefinedError.InvalidElement;

        return await GenerateAccessTokenAsync(user);
    }

    public async Task<Result<string>> CreateRefreshTokenAsync(string username)
    {
        var token = _tokenService.CreateRefreshToken(username);
        var user = _userManager.Users.First(u => u.UserName == username);
        user.RefreshToken = token;
        await _userManager.UpdateAsync(user); 
        return token;
    }

    private async Task<TokenDTO> GenerateAccessTokenAsync(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var accessToken = await _tokenService.CreateAccessTokenAsync(user, userRoles);
        var roles = await _userManager.GetRolesAsync(user);

        return new TokenDTO
        {
            Token = accessToken,
            UserName = user.UserName,
            Roles = roles.ToArray(),
        };
    }
}
