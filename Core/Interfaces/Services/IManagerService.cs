using Core.DTOs.Manager;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface IManagerService
{
    Task<Result<IEnumerable<ManagerReadDTO>>> GetAllAsync();
    Task<Result<IEnumerable<ManagerReadDTO>>> GetManagersInProjectAsync(Guid projectId);
    Task<Result<Guid>> CreateManagerAsync(ManagerCreateDTO dto);
    Task<Result<Guid>> UpdateManagerAsync(ManagerUpdateDTO dto);
    Task<Result<bool>> DeleteManagerAsync(Guid id);
}
