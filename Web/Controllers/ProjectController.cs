using Microsoft.AspNetCore.Mvc;
using Persistence.Data.Contexts;
using Persistence.Repositories;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces.Repositories;
using Core.DTOs.Project;
using Core.Interfaces.Services;

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
        var projects = await _service.GetAllAsync();
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectCreateDTO dto)
    {
        var result = await _service.CreateProjectAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(ProjectUpdateDTO dto)
    {
        var result = await _service.UpdateProjectAsync(dto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _service.DeleteProjectAsync(id);
        return Ok();
    }
}
