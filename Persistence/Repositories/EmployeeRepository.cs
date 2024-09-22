using AutoMapper;
using Core.DTOs.Employee;
using Core.Entities;
using Core.Interfaces.Repositories;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;
internal class EmployeeRepository : BaseCRUDRepository<Employee, EmployeeReadDTO, EmployeeCreateDTO, EmployeeUpdateDTO, DatabaseContext>, IEmployeeRepository
{
    public EmployeeRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }
}
