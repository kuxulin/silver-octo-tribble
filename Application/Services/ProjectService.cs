using Core.DTOs.Project;
using Core.DTOs.TodoTask;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services;

class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateProjectAsync(ProjectCreateDTO dto)
    {
        var duplicate = await GetByNameAsync(dto.Name);

        if (duplicate is not null)
            throw new Exception("There is already such tusk in the project.");

        var result = await _repository.AddAsync(dto);
        return result;
    }

    public async Task DeleteProjectAsync(Guid id)
    {
        var project = await GetByIdAsync(id);

        if (project is null)
            throw new Exception("There is no such project in database.");

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProjectReadDTO>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<ProjectReadDTO> GetByIdAsync(Guid id)
    {
        var projects = await GetAllAsync();
        return projects.FirstOrDefault(p => p.Id == id);
    }

    public async Task<ProjectReadDTO> GetByNameAsync(string name)
    {
        var projects = await GetAllAsync();
        return projects.FirstOrDefault(p => p.Name == name);
    }

    public async Task<Guid> UpdateProjectAsync(ProjectUpdateDTO dto)
    {
        var project = await GetByIdAsync(dto.Id);

        if (project is null)
            throw new Exception("There is no such project in database.");

        var result = await _repository.UpdateAsync(dto);
        return result;
    }
}

