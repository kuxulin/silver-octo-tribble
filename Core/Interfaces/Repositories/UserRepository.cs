using Core.Entities;

namespace Core.Interfaces.Repositories;
public interface IUserRepository
{
    public IQueryable<User> GetAll();
}
