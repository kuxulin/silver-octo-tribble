using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.Contexts;

namespace Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddDbAndEntity(
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
                        options.SignIn.RequireConfirmedEmail = true;
                    })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<DatabaseContext>();       

        return services;
    }
}
