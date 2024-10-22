using Core.DTOs.Manager;
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
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]
public class ManagerController : ControllerBase
{
    private readonly IManagerService _service;

    public ManagerController(IManagerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var todoTasks = await _service.GetAllAsync();
        return Ok(todoTasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ManagerCreateDTO dto)
    {
        var result = await _service.CreateManagerAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(ManagerUpdateDTO dto)
    {
        var result = await _service.UpdateManagerAsync(dto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _service.DeleteManagerAsync(id);
        return Ok();
    }
}
