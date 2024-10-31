using AutoMapper;
using Core.Constants;
using Core.DTOs.Auth;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public AuthService(UserManager<User> userManager, ITokenService tokenService, IImageRepository imageRepository, IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _imageRepository = imageRepository;
        _mapper = mapper;
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

        var user = _mapper.Map<User>(dto);

        IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
        return await GenerateAccessTokenAsync(user);
    }

    public async Task<Result<TokenDTO>> CreateAccessTokenFromRefresh(string oldRefreshToken)
    {
        if(!_tokenService.ValidateToken(oldRefreshToken))
            return DefinedError.InvalidElement;

        string username = _tokenService.GetFieldFromToken(oldRefreshToken,DefinedClaim.Name);

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
        var user = await _userManager.Users.FirstAsync(u => u.UserName == username);
        user.RefreshToken = token;
        await _userManager.UpdateAsync(user); 
        return token;
    }

    public async Task<Result<bool>> DeleteRefreshTokenAsync(string token)
    {
        var username = _tokenService.GetFieldFromToken(token, DefinedClaim.Name);
        var user = await _userManager.Users.FirstAsync(u => u.UserName == username);
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        return true;
    }

    private async Task<TokenDTO> GenerateAccessTokenAsync(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.CreateAccessToken(user, userRoles);
        var roles = await _userManager.GetRolesAsync(user);

        return new TokenDTO
        {
            Token = accessToken,
            UserName = user.UserName!,
            Roles = roles.ToArray(),
            Id = user.Id,
            IsBlocked = user.IsBlocked,
            EmployeeId = user.EmployeeId,
            ManagerId = user.ManagerId,
        };
    }
}
