﻿using Core.DTOs.Employee;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface IEmployeeService
{
    Task<Result<IEnumerable<EmployeeReadDTO>>> GetAllAsync();
    Task<Result<IEnumerable<EmployeeReadDTO>>> GetEmployeesInProjectAsync(Guid projectId);
    Task<Result<Guid>> UpdateEmployeeAsync(EmployeeUpdateDTO dto);
    Task<Result<bool>> DeleteEmployeeAsync(Guid id);
}
