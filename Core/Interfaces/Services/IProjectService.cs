using Core.DTOs.Project;
using Core.DTOs.TodoTask;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface IProjectService
{
    Task<Result<IEnumerable<ProjectReadDTO>>> GetAllAsync();
    Task<Result<ProjectReadDTO>> GetByIdAsync(Guid id);
    Task<Result<ProjectReadDTO>> GetByNameAsync(string name);
    Task<Result<IEnumerable<ProjectReadDTO>>> GetProjectsByManagerIdAsync(Guid id);
    Task<Result<IEnumerable<ProjectReadDTO>>> GetProjectsByEmployeeIdAsync(Guid id);
    Task<Result<ProjectReadDTO>> CreateProjectAsync(ProjectCreateDTO dto);
    Task<Result<ProjectReadDTO>> UpdateProjectAsync(ProjectUpdateDTO dto);
    Task<Result<bool>> DeleteProjectAsync(Guid id);
}
