using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;
internal class ProjectRepository : BaseCRUDRepository<Project, DatabaseContext>, IProjectRepository
{
    public ProjectRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public override IQueryable<Project> GetAll()
    {
        return base.GetAll();
    }

    public IQueryable<Project> GetProjectsByManagerId(Guid id)
    {
        return GetAll().Include(p => p.Managers).Where(p => p.Managers.Any(m => m.Id == id));
    }

    public IQueryable<Project> GetProjectsByEmployeeId(Guid id)
    {
        return GetAll().Include(p => p.Employees).Where(p => p.Employees.Any(e => e.Id == id));
    }

    public async override Task<Project> AddAsync(Project entity, bool isSaved = true)
    {
        if (entity.Managers?.Count > 0)
            _context.Managers.AttachRange(entity.Managers);

        if (entity.Employees?.Count > 0)
            _context.Employees.AttachRange(entity.Employees);

       return await base.AddAsync(entity, isSaved);
    }

    public async override Task<Project> UpdateAsync(Project entity, bool isSaved = true)
    {
        if (entity.Managers?.Count > 0)
            _context.Managers.AttachRange(entity.Managers);

        if (entity.Employees?.Count > 0)
            _context.Employees.AttachRange(entity.Employees);

        return await base.UpdateAsync(entity, isSaved);
    }
}
