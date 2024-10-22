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
