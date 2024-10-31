using Core.DTOs.User;
using Core.Entities;
using Core.Enums;
using Core.ResultPattern;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserReadDTO>> GetUserByIdAsync(int id);
    Task<Result<PagedResult<UserReadDTO>>> GetUsersAsync(UserQueryOptions options);
    Task<Result<bool>> DeleteUsersAsync(IEnumerable<int> ids);
    Task<Result<bool>> ChangeUsersStatusAsync(IEnumerable<int> ids, bool isBlocked);
    Task<Result<bool>> ChangeUserRolesAsync(int id, IEnumerable<AvailableUserRole> roles);
    Task<Result<UsersMetrics>> GetUsersMetricsAsync();
    Task<Result<UserReadDTO>> UpdateUserAsync(UserUpdateDTO userUpdateDTO);
}


