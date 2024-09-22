using Core.DTOs.Project;

namespace Core.Interfaces.Repositories;

public interface IProjectRepository :ICRUDRepository<ProjectReadDTO,ProjectCreateDTO,ProjectUpdateDTO>
{
}

