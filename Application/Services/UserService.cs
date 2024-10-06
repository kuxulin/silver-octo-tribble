using Application.Expressions;
using AutoMapper;
using Core;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.AspNetCore.Http.HttpResults;
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

    public async Task<Result<PagedResult<UserDTO>>> GetUsersAsync(UserQueryOptions options)
    {
        var usersQuery = _repository.GetAll();

        if (!options.SortField.IsNullOrEmpty())
        {
            var sortFunction = ExpressionsGenerator<User>.CreateSortExpression(options.SortField);
            usersQuery = options.SortByDescending ? usersQuery.OrderByDescending(sortFunction) : usersQuery.OrderBy(sortFunction);
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

        var usersAmount = await query.CountAsync();

        var userDtos = _mapper.Map<List<UserDTO>>(usersWithRoles.Select(ur => ur.User));

        for (int i = 0; i < userDtos.Count; i++)
        {
            userDtos[i].Roles = usersWithRoles[i].Roles ?? [];
        }

        return new PagedResult<UserDTO>(userDtos, usersAmount);
    }
}

