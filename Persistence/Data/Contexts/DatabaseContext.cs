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
            .WithMany()
            .HasForeignKey(u => u.ImageId);
    }
}
