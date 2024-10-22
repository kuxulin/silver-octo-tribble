using Core.DTOs.Base;

namespace Core.DTOs.Project;
public class ProjectUpdateDTO : BaseUpdateDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<Guid>? EmployeeIds { get; set; }
    public IEnumerable<Guid>? ManagerIds { get; set; }
}
