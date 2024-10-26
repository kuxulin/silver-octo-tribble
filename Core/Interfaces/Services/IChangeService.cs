using Core.DTOs.Change;

namespace Core.Interfaces.Services;
public interface IChangeService
{
    Task<IEnumerable<ChangeReadDTO>> GetChangesByProjectIdAsync(Guid projectId);
    Task<IEnumerable<ChangeReadDTO>> GetChangesByManagerIdAsync(Guid managerId);
    Task<IEnumerable<ChangeReadDTO>> GetChangesByEmployeeIdAsync(Guid employeeId);
    Task CreateChangeAsync(ChangeCreateDTO dTO); 
}
