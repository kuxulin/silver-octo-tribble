using Core.Entities;

namespace Core.Interfaces.Repositories;
public interface IUserRepository
{
    IQueryable<User> GetAll();
    IQueryable<User> GetPartialUsers();
    Task DeleteUsersAsync(IEnumerable<User> users);
    Task ChangeUsersStatusAsync(IEnumerable<User> users, bool status);
    Task ChangeUserRolesAsync(User user, IEnumerable<string> newUserRoles);
    Task<User> UpdateUserAsync(User user);
}
