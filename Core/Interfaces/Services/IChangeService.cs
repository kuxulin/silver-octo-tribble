using Core.DTOs.Change;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface IChangeService
{
    Task<Result<ChangeReadDTO>> GetChangeByIdAsync(Guid id);
    Task<IEnumerable<ChangeReadDTO>> GetChangesByProjectIdAsync(Guid projectId);
    Task<IEnumerable<ChangeReadDTO>> GetChangesByManagerIdAsync(Guid managerId);
    Task<IEnumerable<ChangeReadDTO>> GetChangesByEmployeeIdAsync(Guid employeeId);
    Task<Guid> CreateChangeAsync(ChangeCreateDTO dTO);
    Task MakeChangeRead(Guid changeId, int userId);
}
