using Application.Services;
using Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITodoTaskService, TodoTaskService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IManagerService, ManagerService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChangeService, ChangeService>();
        return services;
    }
}
