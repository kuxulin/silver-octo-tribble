using AutoMapper;
using Core;
using Core.Constants;
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
        var usersQuery = _repository.GetAll();

        if (!options.SortField.IsNullOrEmpty())
        {
            var sortFunction = (User u) =>
            {
                var property = u.GetType().GetProperty(options.SortField).GetValue(u);
                return property;
            };

            usersQuery = options.SortByDescending ? usersQuery.OrderByDescending(sortFunction).AsQueryable() : usersQuery.OrderBy(sortFunction).AsQueryable();
        }

        var query = usersQuery
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Select(ur => new
            {
                User = ur,
                Roles = ur.UserRoles.Select(ur => ur.Role.Name).ToArray()
            });

        if (options.FilterRoles.Any())
            query = query.Where(u => u.Roles.Intersect(options.FilterRoles).Any());

        if (!options.PartialUserName.IsNullOrEmpty())
            query = query.Where(ur => ur.User.UserName.Contains(options.PartialUserName));

        if (options.IsBlocked.HasValue)
            query = query.Where(ur => ur.User.IsBlocked == options.IsBlocked);

        if (options.StartDate.HasValue)
            query = query.Where(ur => ur.User.CreationDate >= options.StartDate.Value.ToUniversalTime());

        if (options.EndDate.HasValue)
            query = query.Where(ur => ur.User.CreationDate <= options.EndDate.Value.ToUniversalTime());      

        var usersWithRoles = await query
            .Skip(options.GetStartIndex())
            .Take(options.PageSize)
            .ToListAsync();

        var userDtos = _mapper.Map<List<UserDTO>>(usersWithRoles.Select(ur => ur.User));

        for (int i = 0; i < userDtos.Count; i++)
        {
            userDtos[i].Roles = usersWithRoles[i].Roles ?? [];
        }

        return userDtos;
    }
}

