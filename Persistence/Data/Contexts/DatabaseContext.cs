using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Contexts;

public class DatabaseContext : IdentityDbContext
<
    User,
    Role,
    int,
    IdentityUserClaim<int>,
    UserRole,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>
>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TodoTask> ToDoTasks { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<ApplicationImage> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<ApplicationImage>()
         .ToTable("Images");

        modelBuilder.Entity<User>()
            .HasOne(u => u.Image)
            .WithOne(i => i.User)
            .HasForeignKey<ApplicationImage>(u => u.UserId)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Manager)
            .WithOne(m => m.User)
            .HasForeignKey<Manager>(m => m.UserId)
            .IsRequired(false);

        modelBuilder.Entity<User>()
           .HasOne(u => u.Employee)
           .WithOne(e => e.User)
           .HasForeignKey<Employee>(e => e.UserId)
           .IsRequired(false);

        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Projects)
            .WithMany(p => p.Employees)
            .UsingEntity<Dictionary<string, object>>(
                        "ProjectsEmployees",
                        e => e.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                        p => p.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"));

        modelBuilder.Entity<Manager>()
            .HasMany(e => e.Projects)
            .WithMany(p => p.Managers)
            .UsingEntity<Dictionary<string, object>>(
                        "ProjectsManagers",
                        e => e.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                        p => p.HasOne<Manager>().WithMany().HasForeignKey("ManagerId"));
    }
}
