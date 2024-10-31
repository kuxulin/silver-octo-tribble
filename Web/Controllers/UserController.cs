using Core;
using Core.Constants;
using Core.DTOs.User;
using Core.Enums;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserQueryOptions options)
    {
        var result = await _userService.GetUsersAsync(options);
        return Ok(result.Value);
    }

    [HttpDelete]
    [Authorize(Policy = DefinedPolicy.AdminPolicy)]

    public async Task<IActionResult> DeleteUser([FromBody] IEnumerable<int> ids)
    {
        var result = await _userService.DeleteUsersAsync(ids);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok();
    }

    [HttpPatch]
    [Authorize(Policy = DefinedPolicy.AdminPolicy)]

    public async Task<IActionResult> ChangeUsersStatus([FromBody] IEnumerable<int> ids, bool isBlocked)
    {
        var result = await _userService.ChangeUsersStatusAsync(ids, isBlocked);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok();
    }

    [HttpPatch("{id}/roles")]
    [Authorize(Policy = DefinedPolicy.AdminPolicy)]

    public async Task<IActionResult> ChangeUserRoles(int id, IEnumerable<AvailableUserRole> newUserRoles)
    {
        var result = await _userService.ChangeUserRolesAsync(id, newUserRoles);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok();
    }

    [HttpGet("metrics")]
    [Authorize(Policy = DefinedPolicy.AdminPolicy)]

    public async Task<IActionResult> GetUsersMetrics()
    {
        var result = await _userService.GetUsersMetricsAsync();
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
    {
        string authHeader = HttpContext.Request.Headers.Authorization.First()!;
        userUpdateDTO.AccessToken = authHeader["Bearer ".Length..].Trim();
        var result = await _userService.UpdateUserAsync(userUpdateDTO);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }
}

