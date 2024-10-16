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
    private readonly IAuthService _authService;

    public AuthController(IAuthService tokenService)
    {
        _authService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var loginResult = await _authService.Login(dto);

        if (!loginResult.IsSuccess)
            return StatusCode(loginResult.Error.StatusCode, loginResult.Error);

        var refreshTokenResult = await _authService.CreateRefreshTokenAsync(loginResult.Value.UserName);
        AppendCookies(refreshTokenResult.Value);
        return Ok(loginResult.Value);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var accessTokenResult = await _authService.Register(dto);
        
        if(!accessTokenResult.IsSuccess) 
            return StatusCode(accessTokenResult.Error.StatusCode, accessTokenResult.Error);

        var refreshTokenResult = await _authService.CreateRefreshTokenAsync(accessTokenResult.Value.UserName);
        AppendCookies(refreshTokenResult.Value);
        return Ok(accessTokenResult.Value);
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshTokens()
    {
        string oldRefreshToken = GetRefreshToken();
        var accessTokenResult = await _authService.CreateAccessTokenFromRefresh(oldRefreshToken);

        if (!accessTokenResult.IsSuccess)
        {
            return StatusCode(accessTokenResult.Error.StatusCode, accessTokenResult.Error);
        }

        var refreshTokenResult = await _authService.CreateRefreshTokenAsync(accessTokenResult.Value.UserName);
        AppendCookies(refreshTokenResult.Value);
        return Ok(accessTokenResult.Value);
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> LogOut()
    {
        var refreshToken = GetRefreshToken();
        await _authService.DeleteRefreshTokenAsync(refreshToken);
        Response.Cookies.Delete("refreshToken");
        return Ok();
    }

    private string GetRefreshToken()
    {
        return Request.Cookies["refreshToken"];
    }

    private void AppendCookies(string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
        };
        Response.Cookies.Append("refreshToken", token);
    }
}
