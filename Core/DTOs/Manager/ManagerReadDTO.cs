using Core.DTOs.Base;
using Core.DTOs.Project;

namespace Core.DTOs.Manager;
public class ManagerReadDTO :BaseReadDTO
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public IEnumerable<ProjectReadDTO> Projects { get; set; }
}
