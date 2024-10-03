using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.Contexts;
using Persistence.Repositories;

namespace Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddDbAndIdentity(
        this IServiceCollection services,
        string configurationString)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(configurationString));

        services.AddIdentityCore<User>(options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                    })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<DatabaseContext>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IManagerRepository, ManagerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}
