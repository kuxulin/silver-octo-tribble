using Core.DTOs.Manager;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services;
internal class ManagerService : IManagerService
{
    private readonly IManagerRepository _repository;

    public ManagerService(IManagerRepository repository)
    {
        _repository = repository;
    }
    public async Task<Guid> CreateManagerAsync(ManagerCreateDTO dto)
    {
        var duplicate = await GetByNameAsync(dto.FullName);

        if (duplicate is not null)
            throw new Exception("There is already such manager in the project.");

        var result = await _repository.AddAsync(dto);
        return result;
    }

    public async Task DeleteManagerAsync(Guid id)
    {
        var manager = await GetByIdAsync(id);

        if (manager is null)
            throw new Exception("There is no such manager in database.");

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ManagerReadDTO>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<ManagerReadDTO> GetByIdAsync(Guid id)
    {
        var managers = await GetAllAsync();
        return managers.FirstOrDefault(m => m.Id == id);
    }

    public async Task<ManagerReadDTO> GetByNameAsync(string name)
    {
        var managers = await GetAllAsync();
        return managers.FirstOrDefault(m => m.FullName == name);
    }

    public async Task<Guid> UpdateManagerAsync(ManagerUpdateDTO dto)
    {
        var manager = await GetByIdAsync(dto.Id);

        if (manager is null)
            throw new Exception("There is no such manager in database.");

        var result = await _repository.UpdateAsync(dto);
        return result;
    }
}
