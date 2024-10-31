using Core.DTOs.Base;

namespace Core.DTOs.Project;
public class ProjectCreateDTO : BaseCreateDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IEnumerable<Guid> ManagerIds { get; set; } = null!;
    public IEnumerable<Guid>? EmployeeIds { get; set; }
}
