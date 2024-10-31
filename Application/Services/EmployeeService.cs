using AutoMapper;
using Core.Constants;
using Core.DTOs.Employee;
using Core.DTOs.Manager;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
internal class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<EmployeeReadDTO>>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAll().ToListAsync();
        return _mapper.Map<List<EmployeeReadDTO>>(employees);
    }

    public async Task<Result<IEnumerable<EmployeeReadDTO>>> GetEmployeesInProjectAsync(Guid projectId)
    {
        var employees = await _employeeRepository.GetAll()
            .Include(e => e.User)
            .Include(e => e.Projects)
            .Where(e => e.Projects != null && e.Projects.Any(p => p.Id == projectId))
            .ToListAsync();
        return _mapper.Map<List<EmployeeReadDTO>>(employees);
    }

    public async Task<Result<Guid>> CreateEmployeeAsync(EmployeeCreateDTO dto)
    {
        var duplicate = await _employeeRepository.GetAll().FirstOrDefaultAsync(e => e.UserId == dto.UserId);

        if (duplicate is not null)
            return DefinedError.DuplicateEntity;

        var employee = _mapper.Map<Employee>(dto);
        var result = await _employeeRepository.AddAsync(employee);
        return employee.Id;
    }

    public async Task<Result<Guid>> UpdateEmployeeAsync(EmployeeUpdateDTO dto)
    {
        var duplicate = await _employeeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == dto.Id);

        if (duplicate is null)
            return DefinedError.AbsentElement;

        var employee = _mapper.Map<Employee>(dto);
        var result = await _employeeRepository.UpdateAsync(employee);
        return result.Id;
    }
    public async Task<Result<bool>> DeleteEmployeeAsync(Guid id)
    {
        var manager = await _employeeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == id);

        if (manager is null)
            return DefinedError.AbsentElement;

        await _employeeRepository.DeleteAsync(id);
        return true;
    }
}
