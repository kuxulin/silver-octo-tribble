using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IProjectRepository : ICRUDRepository<Project>
{
    IQueryable<Project> GetProjectsByManagerId(Guid id);
    IQueryable<Project> GetProjectsByEmployeeId(Guid id);
}

