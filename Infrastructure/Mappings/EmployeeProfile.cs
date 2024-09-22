using AutoMapper;
using Core.DTOs.Employee;
using Core.Entities;

namespace Infrastructure.Mappings;
internal class EmployeeProfile :Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeCreateDTO, Employee>();
        CreateMap<EmployeeUpdateDTO, Employee>();
        CreateMap<Employee, EmployeeReadDTO>();
    }
}
