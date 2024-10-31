using Core.Constants;
using Core.DTOs.Employee;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy =DefinedPolicy.DefaultPolicy)]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var todoTasks = await _service.GetAllAsync();
        return Ok(todoTasks);
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetEmployeesInProject(Guid projectId)
    {
        var result = await _service.GetEmployeesInProjectAsync(projectId);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(EmployeeCreateDTO dto)
    {
        var result = await _service.CreateEmployeeAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployee(EmployeeUpdateDTO dto)
    {
        var result = await _service.UpdateEmployeeAsync(dto);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        var result = await _service.DeleteEmployeeAsync(id);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error.Message);

        return Ok(result.Value);
    }
}
