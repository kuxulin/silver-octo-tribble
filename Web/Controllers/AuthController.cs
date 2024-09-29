using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces.Services;
using Core.DTOs.Auth;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public AuthController(UserManager<User> userManager, IAuthService tokenService)
    {
        _userManager = userManager;
        _authService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthDTO dto)
    {
        var loginResult = await _authService.Login(dto);

        if (!loginResult.IsSuccess)
            return StatusCode(loginResult.Error.StatusCode, loginResult.Error.Message);

        var refreshTokenResult = await _authService.CreateRefreshTokenAsync(loginResult.Value.UserName);
        Response.Cookies.Append("refreshToken", refreshTokenResult.Value);
        return Ok(loginResult);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthDTO dto)
    {
        var accessTokenResult = await _authService.Register(dto);
        
        if(!accessTokenResult.IsSuccess) 
            return StatusCode(accessTokenResult.Error.StatusCode, accessTokenResult.Error.Message);

        var refreshTokenResult = await _authService.CreateRefreshTokenAsync(accessTokenResult.Value.UserName);
        Response.Cookies.Append("refreshToken", refreshTokenResult.Value);
        return Ok(accessTokenResult);
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshTokens()
    {
        string oldRefreshToken = Request.Cookies["refreshToken"];
        var accessTokenResult = await _authService.CreateAccessTokenFromRefresh(oldRefreshToken);
        var refreshTokenResult = await _authService.CreateRefreshTokenAsync(accessTokenResult.Value.UserName);
        Response.Cookies.Append("refreshToken", refreshTokenResult.Value);
        return Ok(accessTokenResult);
    }
}
