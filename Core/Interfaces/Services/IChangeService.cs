using Core.DTOs.Change;

namespace Core.Interfaces.Services;
public interface IChangeService
{
    Task<IEnumerable<ChangeReadDTO>> GetChangesByProjectIdAsync(Guid projectId);
    Task<IEnumerable<ChangeReadDTO>> GetChangesByUserIdAsync(int userId);
    Task CreateChangeAsync(ChangeCreateDTO dTO); 
}
