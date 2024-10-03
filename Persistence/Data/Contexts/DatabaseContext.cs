using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Contexts;

public class DatabaseContext : IdentityDbContext<User, Role, Guid>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TodoTask> ToDoTasks { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserRole>()
            .ToTable("AspNetUserRoles")
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}
