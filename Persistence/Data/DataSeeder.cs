using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.Contexts;

namespace Persistence.Data;
public static class DataSeeder
{
    public static async Task<IServiceProvider> SeedData(this IServiceProvider provider)
    {
        await SeedAdminAndRoles(provider);
        await SeedTaskStatuses(provider);
        await SeedUsers(provider);
        return provider;
    }

    private static async Task SeedAdminAndRoles(IServiceProvider provider)
    {
        var usersManager = provider.GetRequiredService<UserManager<User>>();
        var rolesManager = provider.GetRequiredService<RoleManager<Role>>();

        if (usersManager.Users.Any(u => u.UserName == "maks"))
            return;

        var context = provider.GetRequiredService<DatabaseContext>();
        Role[] roles = { new() { Name = AvailableUserRole.Admin.ToString() }, new() { Name = AvailableUserRole.Manager.ToString() }, new() { Name = AvailableUserRole.Employee.ToString() } };

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

        var image = new ApplicationImage()
        {
            Name = "User.png",
            Content = GetImageString("User.png"),
        };

        user.Image = image;
        await usersManager.CreateAsync(user, "string");
        await usersManager.AddToRoleAsync(user, roles[0].Name!);
        user.ImageId = image.Id;
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTaskStatuses(IServiceProvider provider)
    {
        var context = provider.GetRequiredService<DatabaseContext>();

        if (context.TaskStatuses.Any())
            return;

        var statuses = new List<TodoTaskStatus>()
        {
            new TodoTaskStatus() { Name = AvailableTaskStatus.Todo.ToString() },
            new TodoTaskStatus() { Name = AvailableTaskStatus.InProgress.ToString() },
            new TodoTaskStatus() { Name = AvailableTaskStatus.OnReview.ToString() },
            new TodoTaskStatus() { Name = AvailableTaskStatus.Completed.ToString() },
        };

        context.TaskStatuses.AddRange(statuses);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsers(IServiceProvider provider)
    {
        var usersManager = provider.GetRequiredService<UserManager<User>>();
        var context = provider.GetRequiredService<DatabaseContext>();

        if (usersManager.Users.Count() > 1)
            return;

        var name = "User.png";
        var content = GetImageString("User.png");
        var managers = new List<User>()
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
                CreationDate = DateTime.UtcNow.AddDays(-12)
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

        var employees = new List<User>()
        {
            new ()
            {
                FirstName = "James",
                LastName = "Brown",
                UserName = "jbrown",
                PhoneNumber = "+380923867117",
                CreationDate = DateTime.UtcNow.AddDays(-1),
            },
            new ()
            {
                FirstName = "Denzel",
                LastName = "Washington",
                UserName = "actor",
                PhoneNumber = "+380921867117",
                CreationDate = DateTime.UtcNow.AddMonths(-2).AddDays(-2),
            },
            new ()
            {
                FirstName = "Rand",
                LastName = "Al`Thor",
                UserName = "dragon",
                PhoneNumber = "+380923857127",
                CreationDate = DateTime.UtcNow.AddDays(-12),
            },
            new ()
            {
                FirstName = "Kate",
                LastName = "Dence",
                UserName = "kdence",
                PhoneNumber = "+380927897017",
                CreationDate = DateTime.UtcNow.AddHours(-6),
            },
            new ()
            {
                FirstName = "Panteleymon",
                LastName = "Kulish",
                UserName = "writer",
                PhoneNumber = "+380927857197",
            },
        };
        var images = new List<ApplicationImage>();

        for (int i = 0; i < employees.Count + managers.Count; i++)
            images.Add(new ApplicationImage()
            {
                Name = name,
                Content = content
            });

        for (int i = 0; i < managers.Count; i++)
        {
            managers[i].Image = images[i];
            var manager = new Manager() { CreationDate = managers[i].CreationDate };
            managers[i].Manager = manager;
            await usersManager.CreateAsync(managers[i], "string");
            await usersManager.AddToRoleAsync(managers[i], AvailableUserRole.Manager.ToString());
            managers[i].ImageId = images[i].Id;
            managers[i].ManagerId = manager.Id;
            context.Update(managers[i]);
        }

        for (int i = 0; i < employees.Count; i++)
        {
            employees[i].Image = images[i + managers.Count];
            var employee = new Employee() { CreationDate = employees[i].CreationDate };
            employees[i].Employee = employee;
            await usersManager.CreateAsync(employees[i], "string");
            await usersManager.AddToRoleAsync(employees[i], AvailableUserRole.Employee.ToString());
            employees[i].ImageId = images[i + managers.Count].Id;
            employees[i].EmployeeId = employee.Id;
            context.Update(employees[i]);
        }

        await context.SaveChangesAsync();
    }

    private static string GetImageString(string imageName)
    {
        string solutionDirectory = GetSolutionDirectoryInfo().FullName;
        string imagePath = Path.Combine(solutionDirectory, imageName);
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        return Convert.ToBase64String(imageBytes);
    }

    private static DirectoryInfo GetSolutionDirectoryInfo()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (!(directory == null || directory.GetFiles("*.sln").Length != 0))
        {
            directory = directory.Parent;
        }

        return directory!;
    }
}
