using Core.DTOs.Employee;

namespace Core.Interfaces.Repositories;
public interface IEmployeeRepository : ICRUDRepository<EmployeeReadDTO,EmployeeCreateDTO,EmployeeUpdateDTO>
{
}
