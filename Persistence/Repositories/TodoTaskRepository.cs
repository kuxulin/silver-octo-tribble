using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;
internal class TodoTaskRepository : BaseCRUDRepository<TodoTask, DatabaseContext>, ITodoTaskRepository
{
    public TodoTaskRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public override IQueryable<TodoTask> GetAll()
    {
        return base.GetAll()
            .Include(t => t.Employee!)
            .ThenInclude(e => e.User);
    }

    public override async Task<TodoTask> AddAsync(TodoTask entity, bool isSaved = true)
    {
        _context.Attach(entity.Project);

        if (entity.Employee is not null)
            _context.Attach(entity.Employee);

        return await base.AddAsync(entity, isSaved);
    }

    public override async Task<TodoTask> UpdateAsync(TodoTask entity, bool isSaved = true)
    {
        if (entity.Status is not null)
            _context.Attach(entity.Status);
        
        return await base.UpdateAsync(entity, isSaved);
    }
}
