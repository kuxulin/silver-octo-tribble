using Core.DTOs;
using Core.Enums;
using Core.Metrics;
using Core.ResultPattern;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<Result<PagedResult<UserDTO>>> GetUsersAsync(UserQueryOptions options);
    Task<Result<bool>> DeleteUsersAsync(IEnumerable<int> ids);
    Task<Result<bool>> ChangeUsersStatusAsync(IEnumerable<int> ids, bool isBlocked);
    Task<Result<bool>> ChangeUserRolesAsync(int id, IEnumerable<AvailableUserRole> roles);
    Task<Result<UsersMetrics>> GetUsersMetricsAsync();
}


