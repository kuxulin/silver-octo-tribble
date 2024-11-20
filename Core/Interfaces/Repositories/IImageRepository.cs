using Core.Entities;

namespace Core.Interfaces.Repositories;
public interface IImageRepository : ICRUDRepository<ApplicationImage>
{
    Task DeleteAsync(IEnumerable<Guid> ids, bool isSaved = true);
}
