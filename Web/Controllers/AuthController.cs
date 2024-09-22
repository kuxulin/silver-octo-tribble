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
        var accessToken = await _authService.Login(dto);

        if (accessToken is null)
            return Unauthorized();

        var refreshToken = _authService.CreateRefreshToken(accessToken.UserName);
        Response.Cookies.Append("refreshToken", refreshToken);
        return Ok(accessToken);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthDTO dto)
    {
        var accessToken = await _authService.Register(dto);
        var refreshToken = _authService.CreateRefreshToken(accessToken.UserName);
        Response.Cookies.Append("refreshToken", refreshToken);
        return Ok(accessToken);
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshTokens()
    {
        string oldRefreshToken = Request.Cookies["refreshToken"];
        var accessToken = await _authService.RefreshTokens(oldRefreshToken);
        var refreshToken = _authService.CreateRefreshToken(accessToken.UserName);
        Response.Cookies.Append("refreshToken", refreshToken);
        return Ok(accessToken);
    }
}
