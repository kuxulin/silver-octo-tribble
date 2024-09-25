using Core.DTOs.Employee;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services;
internal class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }
    public async Task<Guid> CreateEmployeeAsync(EmployeeCreateDTO dto)
    {
        var duplicate = await GetByNameAsync(dto.FullName);

        if (duplicate is not null)
            throw new Exception("There is already such employee in the project.");

        var result = await _repository.AddAsync(dto);
        return result;
    }

    public async Task DeleteEmployeeAsync(Guid id)
    {
        var manager = await GetByIdAsync(id);

        if (manager is null)
            throw new Exception("There is no such employee in database.");

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<EmployeeReadDTO>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<EmployeeReadDTO> GetByIdAsync(Guid id)
    {
        var employees = await GetAllAsync();
        return employees.FirstOrDefault(e => e.Id == id);
    }

    public async Task<EmployeeReadDTO> GetByNameAsync(string name)
    {
        var employees = await GetAllAsync();
        return employees.FirstOrDefault(e => e.FullName == name);
    }

    public async Task<Guid> UpdateEmployeeAsync(EmployeeUpdateDTO dto)
    {
        var employee = await GetByIdAsync(dto.Id);

        if (employee is null)
            throw new Exception("There is no such employee in database.");

        var result = await _repository.UpdateAsync(dto);
        return result;
    }
}
