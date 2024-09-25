using Core.DTOs.TodoTask;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data.Contexts;
using Persistence.Repositories;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoTaskController : ControllerBase
{
    private readonly ITodoTaskService _service;

    public TodoTaskController(ITodoTaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var todoTasks= await _service.GetAllAsync();
        return Ok(todoTasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(TodoTaskCreateDTO dto)
    {
        var result = await _service.CreateTodoTaskAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(TodoTaskUpdateDTO dto)
    {
        var result = await _service.UpdateTodoTaskAsync(dto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _service.DeleteTodoTaskAsync(id);
        return Ok();
    }
}

