using Application.Expressions;
using AutoMapper;
using Core;
using Core.Constants;
using Core.DTOs.User;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Metrics;
using Core.ResultPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace Application.Services;

class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IImageRepository _imageRepository;

    public UserService(IUserRepository repository, IMapper mapper,IImageRepository imageRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _imageRepository = imageRepository;
    }

    public async Task<Result<UserReadDTO>> GetUserByIdAsync(int id)
    {
        var user = await GetUserById(id);

        if (user is null)
            return DefinedError.AbsentElement;

        return _mapper.Map<UserReadDTO>(user);
    }

    public async Task<Result<PagedResult<UserReadDTO>>> GetUsersAsync(UserQueryOptions options)
    {
        var usersQuery = _repository.GetAll();

        if (!options.SortField.IsNullOrEmpty())
        {
            var sortFunction = ExpressionsGenerator<User>.CreateSortExpression(options.SortField);
            usersQuery = options.SortByDescending ? usersQuery.OrderByDescending(sortFunction) : usersQuery.OrderBy(sortFunction);
        }

        var query = usersQuery.Include(u => u.Image).Include(u => u.UserRoles).Select(ur => new
        {
            User = ur,
            ur.UserRoles,
        });

        if (!options.FilterRoleIds.IsNullOrEmpty())
            query = query.Where(ur => ur.UserRoles.Select(ur => ur.RoleId).Intersect(options.FilterRoleIds).Any());

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
        var userDtos = _mapper.Map<IEnumerable<UserReadDTO>>(usersWithRoles.Select(ur => ur.User));
        return new PagedResult<UserReadDTO>(userDtos, usersAmount);
    }

    public async Task<Result<bool>> DeleteUsersAsync(IEnumerable<int> ids)
    {
        var users = await _repository.GetAll().Where(u => ids.Contains(u.Id)).ToListAsync();

        if (users is null || users.Count < ids.Count())
            return DefinedError.AbsentElement;

        await _repository.DeleteUsersAsync(users);
        return true;
    }

    public async Task<Result<bool>> ChangeUsersStatusAsync(IEnumerable<int> ids, bool isBlocked)
    {
        var users = await _repository.GetAll().Where(u => ids.Contains(u.Id)).ToListAsync();

        if (users == null || users.Count < ids.Count())
            return DefinedError.AbsentElement;

        await _repository.ChangeUsersStatusAsync(users, isBlocked);
        return true;
    }

    private async Task<User> GetUserById(int id)
    {
        return await _repository.GetAll().Include(u => u.UserRoles).Include(u => u.Image).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Result<bool>> ChangeUserRolesAsync(int id, IEnumerable<AvailableUserRole> roles)
    {
        foreach (var role in roles)
        {
            if (!Enum.IsDefined(typeof(AvailableUserRole), role))
            {
                return DefinedError.InvalidElement;
            }
        }

        var user = await GetUserById(id);

        if (user == null)
            return DefinedError.AbsentElement;

        await _repository.ChangeUserRolesAsync(user, roles.Select(r => r.ToString()).ToArray());

        return true;
    }

    public async Task<Result<UsersMetrics>> GetUsersMetricsAsync()
    {
        var query = _repository.GetAll();

        var adminQuery = query.Include(u => u.UserRoles)
            .Where(u => u.UserRoles.Select(ur => ur.RoleId).Contains(((int)AvailableUserRole.Admin)));

        var blockedQuery = query.Where(u => u.IsBlocked);

        var totalCount = await query.CountAsync();
        var adminCount = await adminQuery.CountAsync();
        var blockedCount = await blockedQuery.CountAsync();


        return new UsersMetrics()
        {
            TotalCount = totalCount,
            AdminCount = adminCount,
            BlockedCount = blockedCount
        };
    }

    public async Task<Result<UserReadDTO>> UpdateUserAsync(UserUpdateDTO userUpdateDTO)
    {
        var user = await GetUserById(userUpdateDTO.Id);

        if (user == null)
            return DefinedError.AbsentElement;

        var possibleDuplicate = await _repository.GetAll().FirstOrDefaultAsync(u => u.UserName == userUpdateDTO.UserName);

        if (possibleDuplicate is not null && user.Id != possibleDuplicate.Id)
            return DefinedError.DuplicateEntity;

        if (userUpdateDTO.UserName is not null)
            user.UserName = userUpdateDTO.UserName;

        if (userUpdateDTO.FirstName is not null)
            user.FirstName = userUpdateDTO.FirstName;

        if (userUpdateDTO.LastName is not null)
            user.LastName = userUpdateDTO.LastName;

        if (userUpdateDTO.PhoneNumber is not null)
            user.PhoneNumber = userUpdateDTO.PhoneNumber;

        if(userUpdateDTO.ImageDto is not null && userUpdateDTO.ImageDto.Content != user.Image.Content)
        {
            var newImage = _mapper.Map<ApplicationImage>(userUpdateDTO.ImageDto);
            await _imageRepository.ReplaceImage(user.Image, newImage);
            user.Image = newImage;
        }

        var updatedUser = await _repository.UpdateUserAsync(user);
        return _mapper.Map<UserReadDTO>(updatedUser);
    }
}

