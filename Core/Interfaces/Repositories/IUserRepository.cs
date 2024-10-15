using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Repositories;
public interface IUserRepository
{
    IQueryable<User> GetAll();
    Task DeleteUsersAsync(IEnumerable<User> users);
    Task ChangeUsersStatusAsync(IEnumerable<User> users, bool status);
    Task ChangeUserRolesAsync(User user, IEnumerable<string> newUserRoles);
    Task<User> UpdateUserAsync(User user);
}
