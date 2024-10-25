using Core.DTOs.User;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;

class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;
    private readonly UserManager<User> _userManager;

    public UserRepository(DatabaseContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IQueryable<User> GetAll()
    {
        return _context.Users
            .Include(u => u.Image)
            .Include(u => u.Manager)
            .Include(u => u.Employee)
            .Include(u => u.UserRoles);
    }

    public IQueryable<User> GetPartialUsers()
    {
        return _context.Users;
    }

    public async Task DeleteUsersAsync(IEnumerable<User> users)
    {
        _context.Users.RemoveRange(users);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeUsersStatusAsync(IEnumerable<User> users, bool status)
    {
        foreach (var user in users)
            user.IsBlocked = status;

        _context.Users.UpdateRange(users);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeUserRolesAsync(User user, IEnumerable<string> newUserRoles)
    {
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRolesAsync(user, newUserRoles);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        await _userManager.UpdateAsync(user);
        return user;
    }
}

