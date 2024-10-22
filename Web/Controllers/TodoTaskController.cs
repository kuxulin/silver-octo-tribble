using Core.Constants;
using Core.DTOs.TodoTask;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]
public class TodoTaskController : ControllerBase
{
    private readonly ITodoTaskService _service;

    public TodoTaskController(ITodoTaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetTasksByProjectId(Guid projectId)
    {
        var result = await _service.GetByProjectIdAsync(projectId);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(TodoTaskCreateDTO dto)
    {
        var result = await _service.CreateTodoTaskAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask(TodoTaskUpdateDTO dto)
    {
        var result = await _service.UpdateTodoTaskAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var result = await _service.DeleteTodoTaskAsync(id);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok();
    }
}

