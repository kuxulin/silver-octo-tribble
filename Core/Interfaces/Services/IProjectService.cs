using Core.DTOs.Project;
using Core.DTOs.TodoTask;

namespace Core.Interfaces.Services;
public interface IProjectService
{
    Task<IEnumerable<ProjectReadDTO>> GetAllAsync();
    Task<ProjectReadDTO> GetByIdAsync(Guid id);
    Task<ProjectReadDTO> GetByNameAsync(string name);
    Task<Guid> CreateProjectAsync(ProjectCreateDTO dto);
    Task<Guid> UpdateProjectAsync(ProjectUpdateDTO dto);
    Task DeleteProjectAsync(Guid id);
}
