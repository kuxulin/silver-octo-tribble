using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Data;
public static class DataSeeder
{
    public static async Task<IServiceProvider> SeedAdminAndRoles(this IServiceProvider provider)
    {
        var usersManager = provider.GetRequiredService<UserManager<User>>();
        var rolesManager = provider.GetRequiredService<RoleManager<Role>>();

        if (usersManager.Users.Any(u => u.UserName == "maks"))
            return provider;

        Role[] roles = { new() { Name = "Admin" }, new() { Name = "Employee" }, new() { Name = "Manager" } };

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

        await usersManager.CreateAsync(user, "string"); // TODO perhaps transactionc can be user here
        await usersManager.AddToRoleAsync(user, roles[0].Name);
        return provider;
    }

    public static async Task<IServiceProvider> SeedUsers(this IServiceProvider provider)
    {
        var usersManager = provider.GetRequiredService<UserManager<User>>();

        if (usersManager.Users.Count() > 1)
            return provider;

        var users = new List<User>()
        {
            new ()
            {
                FirstName = "String",
                LastName = "String",
                UserName = "string",
                PhoneNumber = "+380953867117",
                CreationDate = DateTime.UtcNow.AddDays(-3),
            },
            new ()
            {
                FirstName = "Bruce",
                LastName = "Wayne",
                UserName = "batman",
                PhoneNumber = "+380923867117",
                CreationDate = DateTime.UtcNow.AddMonths(-1).AddDays(-2),
            },
            new ()
            {
                FirstName = "Danylo",
                LastName = "Shalak",
                UserName = "ajeoss",
                PhoneNumber = "+380923857117",
                CreationDate = DateTime.UtcNow.AddDays(-12),
            },
            new ()
            {
                FirstName = "Maksym",
                LastName = "Zaika",
                UserName = "mazsek",
                PhoneNumber = "+380927857117",
                CreationDate = DateTime.UtcNow.AddHours(-6),
            },
            new ()
            {
                FirstName = "Roman",
                LastName = "Tymoshchuk",
                UserName = "nctw",
                PhoneNumber = "+380927157197",
            },
        };

        var roles = new[] { "Manager", "Manager", "Employee", "Employee", "Employee" };

        foreach (var user in users)
        {
            await usersManager.CreateAsync(user, "string");
            await usersManager.AddToRoleAsync(user, roles[0]);
        }

        return provider;
    }
}
