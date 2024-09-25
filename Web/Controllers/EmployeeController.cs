using Core.DTOs.Employee;
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
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service; 

    public EmployeeController(IEmployeeService service)
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
    public async Task<IActionResult> CreateProject(EmployeeCreateDTO dto)
    {
        var result = await _service.CreateEmployeeAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject(EmployeeUpdateDTO dto)
    {
        var result = await _service.UpdateEmployeeAsync(dto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _service.DeleteEmployeeAsync(id);
        return Ok();
    }
}
