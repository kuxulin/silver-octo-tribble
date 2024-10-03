using Core.DTOs;
using Core.ResultPattern;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<Result<IEnumerable<UserDTO>>> GetUsers(UserQueryOptions options);
}


