using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;

internal class ChangeRepository :BaseCRUDRepository<Change, DatabaseContext>,IChangeRepository
{
    public ChangeRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public override IQueryable<Change> GetAll()
    {
        return base.GetAll()
            .Include(c => c.Creator)
            .Include(c => c.Task)
            .ThenInclude(t => t.Employee);
    }

    public override async Task<Change> AddAsync(Change entity, bool isSaved = true)
    {
        return await base.AddAsync(entity, isSaved);
    }
}

