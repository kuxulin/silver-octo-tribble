using AutoMapper;
using Core;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace Application.Services;

class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<UserDTO>>> GetUsers(UserQueryOptions options)
    {
        var query = _repository.GetAll();

        if (options.FilterRoles.Any())
        {
            var roles = _context.Roles.Where(role => options.FilterRoles.Contains(role.Name));

            query = query
                .Join(_context.UserRoles,
                      user => user.Id,
                      userRole => userRole.UserId,
                      (user, userRole) => new { user, userRole })
                .Where(uur => roles.Any(role => role.Id == uur.userRole.RoleId))
                .Select(uur => uur.user);
        }

        query = !options.PartialUserName.IsNullOrEmpty() ? query.Where(u => u.UserName.Contains(options.PartialUserName)) : query;

        query = options.IsBlocked.HasValue ? query.Where(u => u.IsBlocked == options.IsBlocked) : query;

        query = options.StartDate.HasValue ? query.Where(u => u.CreationDate >= options.StartDate.Value.ToUniversalTime()) : query;

        query = options.EndDate.HasValue ? query.Where(u => u.CreationDate <= options.EndDate.Value.ToUniversalTime()) : query;

        if (!options.SortField.IsNullOrEmpty())
        {
            var sort = (User u) =>
            {
                var property = u.GetType().GetProperty(options.SortField).GetValue(u);
                return property;
            };

            query = options.SortByDescending ? query.OrderByDescending(sort).AsQueryable() : query.OrderBy(sort).AsQueryable();
        }

        var usersWithRolesQuery = query
            .Join(_context.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { user, userRole })
            .Join(_context.Roles,
                    uur => uur.userRole.RoleId,
                    role => role.Id,
                    (userUserRole, role) => new
                    {
                        User = userUserRole.user,
                        Role = role.Name
                    })
            .GroupBy(ur => ur.User)
            .Select(group => new
            {
                User = group.Key,
                Roles = group.Select(r => r.Role).ToList()
            });

        var usersWithRoles = await usersWithRolesQuery
            .Skip(options.GetStartIndex())
            .Take(options.PageSize)
            .ToListAsync();

        var userDtos = _mapper.Map<List<UserDTO>>(usersWithRoles.Select(ur => ur.User));
        
        for (int i = 0; i < userDtos.Count; i++)
        {
            userDtos[i].Roles = usersWithRoles[i].Roles?.ToArray() ?? [];
        }

        return userDtos;
    }
}

