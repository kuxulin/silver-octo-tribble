using Core.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Employee;
public class EmployeeCreateDTO :BaseCreateDTO
{
    public int UserId { get; set; }
}
