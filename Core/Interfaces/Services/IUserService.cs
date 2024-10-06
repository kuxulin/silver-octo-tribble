using Core.DTOs;
using Core.ResultPattern;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<Result<PagedResult<UserDTO>>> GetUsersAsync(UserQueryOptions options);
}


