using Core.DTOs.TodoTask;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data.Contexts;
using Persistence.Repositories;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TodoTaskController : ControllerBase
{
    private readonly ITodoTaskRepository _repository;

    public TodoTaskController(ITodoTaskRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var todoTasks= await _repository.GetAllAsync();
        return Ok(todoTasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(TodoTaskCreateDTO dto)
    {
        var result = await _repository.AddAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(TodoTaskUpdateDTO dto)
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

