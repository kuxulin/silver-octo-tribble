using Core.Constants;
using Core.DTOs.Project;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    [HttpGet("manager/{managerId}")]
    public async Task<IActionResult> GetProjectsByManagerId(Guid managerId)
    {
        var result = await _service.GetProjectsByManagerIdAsync(managerId);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetProjectsByEmployeeId(Guid employeeId)
    {
        var result = await _service.GetProjectsByEmployeeIdAsync(employeeId);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectCreateDTO dto)
    {
        var result = await _service.CreateProjectAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(ProjectUpdateDTO dto)
    {
        var result = await _service.UpdateProjectAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var result = await _service.DeleteProjectAsync(id);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok();
    }
}
