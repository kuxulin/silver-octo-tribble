using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Data;
public static class AdminSeeder
{
    public static async Task<IServiceProvider> SeedAdmin(this IServiceProvider provider)
    {

        var usersManager = provider.GetRequiredService<UserManager<User>>();
        var rolesManager = provider.GetRequiredService<RoleManager<Role>>();

        if (usersManager.Users.Any(u => u.UserName == "maks"))
            return provider;

        Role[] roles = { new() { Name="Admin" }, new() { Name = "Employee" }, new() { Name = "Manager" } };

        foreach (var role in roles)
        {
            await rolesManager.CreateAsync(role);
        }

        var user = new User
        {
            FirstName = "Maksym",
            LastName = "Moroz",
            UserName = "maks",
            PhoneNumber = "+380953867137",
        };

        await usersManager.CreateAsync(user, "string");
        await usersManager.AddToRoleAsync(user, roles[0].Name);
        return provider;
    }
}
