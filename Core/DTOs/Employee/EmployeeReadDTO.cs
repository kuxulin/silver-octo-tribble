using Core.DTOs.Base;

namespace Core.DTOs.Employee;
public class EmployeeReadDTO :BaseReadDTO
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
