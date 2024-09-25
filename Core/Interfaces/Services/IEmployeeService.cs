using Core.DTOs.Employee;

namespace Core.Interfaces.Services;
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeReadDTO>> GetAllAsync();
    Task<EmployeeReadDTO> GetByIdAsync(Guid id);
    Task<EmployeeReadDTO> GetByNameAsync(string name);
    Task<Guid> CreateEmployeeAsync(EmployeeCreateDTO dto);
    Task<Guid> UpdateEmployeeAsync(EmployeeUpdateDTO dto);
    Task DeleteEmployeeAsync(Guid id);
}
