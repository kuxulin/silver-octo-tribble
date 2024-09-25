using AutoMapper;
using Core.DTOs.Project;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;
internal class ProjectRepository : BaseCRUDRepository<Project, ProjectReadDTO, ProjectCreateDTO, ProjectUpdateDTO, DatabaseContext>, IProjectRepository
{
    public ProjectRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }

    protected new IQueryable<Project> GetAll()
    {
        return base.GetAll()
            .Include(p => p.Employees)
            .Include(p => p.Managers)
            .Include(p => p.ToDoTasks);
    }


}
