using Core.DTOs.Base;
using Core.DTOs.Employee;
using Core.DTOs.Manager;
using Core.DTOs.TodoTask;

namespace Core.DTOs.Project;
public class ProjectReadDTO : BaseReadDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IEnumerable<TodoTaskReadDTO>? Tasks { get; set; }
    public IEnumerable<ManagerReadDTO> Managers { get; set; } = null!;
    public IEnumerable<EmployeeReadDTO>? Employees { get; set; }
}
