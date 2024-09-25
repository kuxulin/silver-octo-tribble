using Core.DTOs.Manager;

namespace Core.Interfaces.Services;
public interface IManagerService
{
    Task<IEnumerable<ManagerReadDTO>> GetAllAsync();
    Task<ManagerReadDTO> GetByIdAsync(Guid id);
    Task<ManagerReadDTO> GetByNameAsync(string name);
    Task<Guid> CreateManagerAsync(ManagerCreateDTO dto);
    Task<Guid> UpdateManagerAsync(ManagerUpdateDTO dto);
    Task DeleteManagerAsync(Guid id);
}
