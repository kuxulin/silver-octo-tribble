using Core;
using Core.Constants;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public async Task<IActionResult> DeleteUser([FromBody]IEnumerable<int> ids)
    {
        var result = await _userService.DeleteUsersAsync(ids);

        if(!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeUsersStatus([FromBody] IEnumerable<int> ids, bool isBlocked)
    {
        var result = await _userService.ChangeUsersStatusAsync(ids,isBlocked);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok();
    }

    [HttpPatch("{id}/roles")]
    public async Task<IActionResult> ChangeUserRoles(int id, IEnumerable<AvailableUserRole> newUserRoles)
    {
        var result = await _userService.ChangeUserRolesAsync(id, newUserRoles);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok();
    }

    [HttpGet("metrics")]
    public async Task<IActionResult> GetUsersMetrics()
    {
        var result = await _userService.GetUsersMetricsAsync();
        return Ok(result.Value);
    }
}

