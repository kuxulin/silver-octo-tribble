using Core.DTOs.Base;

namespace Core.DTOs.Employee;
public class EmployeeCreateDTO : BaseCreateDTO
{
    public int UserId { get; set; }
}
