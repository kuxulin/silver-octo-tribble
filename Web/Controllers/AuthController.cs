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
        var tokens = await _authService.Login(dto);

        if (tokens is null)
            return Unauthorized();

        Response.Cookies.Append("refreshToken", tokens.RefreshToken);
        return Ok(tokens);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthDTO dto)
    {
        var tokens = await _authService.Register(dto);
        Response.Cookies.Append("refreshToken",tokens.RefreshToken);
        return Ok(tokens);
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshTokens()
    {
        string oldRefreshToken = Request.Cookies["refreshToken"];
        var tokens = await _authService.RefreshTokens(oldRefreshToken);
        return Ok(tokens);
    }
}
