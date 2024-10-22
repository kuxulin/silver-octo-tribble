using Core.Constants;
using Core.DTOs.Manager;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]
public class ManagerController : ControllerBase
{
    private readonly IManagerService _service;

    public ManagerController(IManagerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetManagers()
    {
        var todoTasks = await _service.GetAllAsync();
        return Ok(todoTasks);
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetManagersInProject(Guid projectId)
    {
        var result = await _service.GetManagersInProjectAsync(projectId);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateManager(ManagerCreateDTO dto)
    {
        var result = await _service.CreateManagerAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateManager(ManagerUpdateDTO dto)
    {
        var result = await _service.UpdateManagerAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteManager(Guid id)
    {
        var result = await _service.DeleteManagerAsync(id);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok();
    }
}
