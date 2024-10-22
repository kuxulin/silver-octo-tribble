using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces.Repositories;
using Core.Entities;
using Core.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Core.DTOs.Base;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Repositories;

public abstract class BaseCRUDRepository<TEntity, TContext>
    : ICRUDRepository<TEntity>
    where TEntity : BaseEntity
    where TContext : IdentityDbContext
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
    protected readonly TContext _context;
    protected readonly IMapper _mapper;

    public BaseCRUDRepository(TContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>();
    }

    public async virtual Task<TEntity> AddAsync(TEntity entity, bool isSaved = true)
    {
        _context.Set<TEntity>().Add(entity);

        if (isSaved)
            await _context.SaveChangesAsync();

        return entity;
    }

    public async virtual Task<TEntity> UpdateAsync(TEntity entity, bool isSaved = true)
    {
        _context.Set<TEntity>().Update(entity);

        if (isSaved)
            await _context.SaveChangesAsync();

        return entity;
    }

    public async virtual Task DeleteAsync(Guid id, bool isSaved = true)
    {
        var entity = await GetAll().FirstAsync(e => e.Id == id);
        _context.Set<TEntity>().Remove(entity);

        if (isSaved)
            await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
