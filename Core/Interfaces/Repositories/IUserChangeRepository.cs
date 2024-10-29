using Core.Entities;

namespace Core.Interfaces.Repositories;
public interface IUserChangeRepository
{
    Task AddUserChangeAsync(UserChange userChange, bool isSaved = true);
    Task SaveChangesAsync();
}
