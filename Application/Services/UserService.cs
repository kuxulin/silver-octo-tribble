using Application.Expressions;
using AutoMapper;
using Core;
using Core.Constants;
using Core.DTOs.User;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace Application.Services;

class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;
    private readonly ITokenService _tokenService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IManagerRepository _managerRepository;

    public UserService(IUserRepository userRepository, IMapper mapper, IImageService imageService, IEmployeeRepository employeeRepository, IManagerRepository managerRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _imageService = imageService;
        _tokenService = tokenService;
        _employeeRepository = employeeRepository;
        _managerRepository = managerRepository;
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
        var usersQuery = _userRepository.GetAll();

        if (!options.SortField.IsNullOrEmpty())
        {
            var sortFunction = ExpressionsGenerator<User>.CreateSortExpression(options.SortField!);
            usersQuery = options.SortByDescending ? usersQuery.OrderByDescending(sortFunction) : usersQuery.OrderBy(sortFunction);
        }

        var query = usersQuery
            .Select(ur => new
            {
                User = ur,
                ur.UserRoles,
            });

        if (!options.FilterRoleIds.IsNullOrEmpty())
            query = query.Where(ur => ur.UserRoles.Select(ur => ur.RoleId).Intersect(options.FilterRoleIds!).Any());

        if (!options.PartialUserName.IsNullOrEmpty())
            query = query.Where(ur => ur.User.UserName!.Contains(options.PartialUserName!));

        if (options.IsBlocked.HasValue)
            query = query.Where(ur => ur.User.IsBlocked == options.IsBlocked);

        if (options.StartDate.HasValue)
            query = query.Where(ur => ur.User.CreationDate >= options.StartDate.Value.ToUniversalTime());

        if (options.EndDate.HasValue)
            query = query.Where(ur => ur.User.CreationDate <= options.EndDate.Value.ToUniversalTime());

        var usersAmount = await query.CountAsync();
        query = query.Skip(options.GetStartIndex());

        if (options.PageSize.HasValue)
            query = query.Take(options.PageSize.Value);

        var usersWithRoles = await query.ToListAsync();
        var userDtos = _mapper.Map<IEnumerable<UserReadDTO>>(usersWithRoles.Select(ur => ur.User));
        return new PagedResult<UserReadDTO>(userDtos, usersAmount);
    }

    public async Task<Result<bool>> DeleteUsersAsync(IEnumerable<int> ids)
    {
        var users = await _userRepository.GetAll().Where(u => ids.Contains(u.Id)).ToListAsync();

        if (users is null || users.Count < ids.Count())
            return DefinedError.AbsentElement;

        await _imageService.DeleteImagesAsync(users.Select(u => u.ImageId));
        await _userRepository.DeleteUsersAsync(users);
        return true;
    }

    public async Task<Result<bool>> ChangeUsersStatusAsync(IEnumerable<int> ids, bool isBlocked)
    {
        var users = await _userRepository.GetAll().Where(u => ids.Contains(u.Id)).ToListAsync();

        if (users == null || users.Count < ids.Count())
            return DefinedError.AbsentElement;

        await _userRepository.ChangeUsersStatusAsync(users, isBlocked);
        return true;
    }

    private async Task<User?> GetUserById(int id)
    {
        return await _userRepository.GetAll().Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Result<bool>> ChangeUserRolesAsync(int id, IEnumerable<AvailableUserRole> newRoles)
    {
        foreach (var role in newRoles)
        {
            if (!Enum.IsDefined(typeof(AvailableUserRole), role))
            {
                return DefinedError.InvalidElement;
            }
        }

        var user = await GetUserById(id);

        if (user == null)
            return DefinedError.AbsentElement;

        await _userRepository.ChangeUserRolesAsync(user, newRoles.Select(r => r.ToString()).ToArray());
        await ConfigureNewRoles(user, newRoles);
        return true;
    }

    private async Task ConfigureNewRoles(User user, IEnumerable<AvailableUserRole> newRoles)
    {
        Manager? manager = user.Manager;
        Employee? employee = user.Employee;
        if (newRoles.Contains(AvailableUserRole.Manager) && user.ManagerId is null)
        {
            manager = new Manager() { UserId = user.Id };
            await _managerRepository.AddAsync(manager, isSaved: false);

        }

        if (!newRoles.Contains(AvailableUserRole.Manager) && user.ManagerId is not null)
        {
            await _managerRepository.DeleteAsync(user.ManagerId.Value, isSaved: false);
            manager = null;
        }

        if (newRoles.Contains(AvailableUserRole.Employee) && user.EmployeeId is null)
        {
            employee = new Employee() { UserId = user.Id };
            await _employeeRepository.AddAsync(employee, isSaved: false);
        }

        if (!newRoles.Contains(AvailableUserRole.Employee) && user.EmployeeId is not null)
        {
            await _employeeRepository.DeleteAsync(user.EmployeeId.Value, isSaved: false);
            employee = null;
        }

        user.Manager = manager;
        user.Employee = employee;
        await _managerRepository.SaveChangesAsync();
        user.ManagerId = manager?.Id;
        user.EmployeeId = employee?.Id;
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task<Result<UsersMetrics>> GetUsersMetricsAsync()
    {
        var query = _userRepository.GetAll();

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
        var usernameFromToken = _tokenService.GetFieldFromToken(userUpdateDTO.AccessToken!, DefinedClaim.Name);

        if (user is null)
            return DefinedError.AbsentElement;

        if (user.UserName != usernameFromToken)
            return DefinedError.ForbiddenAction;

        var possibleDuplicate = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.UserName == userUpdateDTO.UserName);

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

        
        var imageIdToDelete = Guid.Empty;

        if (userUpdateDTO.ImageDto is not null )
        {
            var areEqual = (await _imageService.AreEqual(userUpdateDTO.ImageDto, user.ImageId)).Value!;

            if(!areEqual)
            {
                var newImageResult = await _imageService.AddImageAsync(userUpdateDTO.ImageDto);

                if (!newImageResult.IsSuccess)
                    return newImageResult.Error!;

                imageIdToDelete = user.ImageId;
                user.ImageId = newImageResult.Value;
            } 
        }

        var updatedUser = await _userRepository.UpdateUserAsync(user);

        if (imageIdToDelete != Guid.Empty)
            await _imageService.DeleteImagesAsync([imageIdToDelete]);

        return _mapper.Map<UserReadDTO>(updatedUser);
    }
}