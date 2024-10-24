using Core.Constants;
using Core.DTOs.Change;
using Core.DTOs.TodoTask;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]
public class TodoTaskController : ControllerBase
{
    private readonly ITodoTaskService _taskService;
    private readonly IChangeService _changeService;
    private readonly ITokenService _tokenService;

    public TodoTaskController(ITodoTaskService taskService, IChangeService changeService, ITokenService tokenService)
    {
        _taskService = taskService;
        _changeService = changeService;
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var result = await _taskService.GetAllAsync();
        return Ok(result.Value);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetTasksByProjectId(Guid projectId)
    {
        var result = await _taskService.GetByProjectIdAsync(projectId);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(TodoTaskCreateDTO dto)
    {
        var result = await _taskService.CreateTodoTaskAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        await CreateChange(DefinedAction.Create, result.Value.ProjectId, result.Value.Title, result.Value.Id);
        return Ok(result.Value);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask(TodoTaskUpdateDTO dto)
    {
        var result = await _taskService.UpdateTodoTaskAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        await CreateChange(DefinedAction.Update, result.Value.ProjectId, result.Value.Title, result.Value.Id);
        return Ok(result.Value);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeTaskEmployee(Guid taskId, Guid? employeeId)
    {
        var result = await _taskService.ChangeTaskEmployeeAsync(taskId,employeeId);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        await CreateChange(DefinedAction.Assign, result.Value.ProjectId, result.Value.Title, result.Value.Id);
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var result = await _taskService.DeleteTodoTaskAsync(id);

        if (!result.IsSuccess)
            return StatusCode(result.Error.StatusCode, result.Error.Message);

        await CreateChange(DefinedAction.Delete, result.Value.ProjectId, result.Value.Title);
        return Ok();
    }

    private async Task CreateChange(string action, Guid projectId, string title, Guid? taskId = null)
    {
        if (title is null && taskId is null)
            throw new ArgumentNullException(nameof(title));

        var changeDto = new ChangeCreateDTO()
        {
            CreatorId = GetIdFromToken(),
            TaskTitle = title,
            ActionType = action,
            TaskId = taskId,
            ProjectId = projectId
        };

        await _changeService.CreateChangeAsync(changeDto);
    }

    private int GetIdFromToken()
    {
        var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault();
        var accessToken = authHeader["Bearer ".Length..].Trim();
        return int.Parse(_tokenService.GetFieldFromToken(accessToken, DefinedClaim.Id));
    }
}