using Microsoft.AspNetCore.Mvc;
using Persistence.Data.Contexts;
using Persistence.Repositories;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces.Repositories;
using Core.DTOs.Project;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _repository;

    public ProjectController(IProjectRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _repository.GetAllAsync();
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectCreateDTO dto)
    {
        var result = await _repository.AddAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(ProjectUpdateDTO dto)
    {
        var result = await _repository.UpdateAsync(dto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}
