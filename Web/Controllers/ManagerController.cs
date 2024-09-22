using Core.DTOs.Manager;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data.Contexts;
using Persistence.Repositories;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ManagerController : ControllerBase
{
    private readonly IManagerRepository _repository;

    public ManagerController(IManagerRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var todoTasks = await _repository.GetAllAsync();
        return Ok(todoTasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ManagerCreateDTO dto)
    {
        var result = await _repository.AddAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(ManagerUpdateDTO dto)
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
