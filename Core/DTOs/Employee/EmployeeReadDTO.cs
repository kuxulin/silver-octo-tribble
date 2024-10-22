using Core.DTOs.Base;
using Core.DTOs.User;

namespace Core.DTOs.Employee;
public class EmployeeReadDTO :BaseReadDTO
{
    public int UserId { get; set; }
    public UserReadDTO User { get; set; }
}
