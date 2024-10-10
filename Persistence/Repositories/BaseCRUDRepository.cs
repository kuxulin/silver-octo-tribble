using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces.Repositories;
using Core.Entities;
using Core.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Core.DTOs.Base;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Repositories;

public abstract class BaseCRUDRepository<TEntity, TReadDto,TCreateDto,TUpdateDto, TContext> 
    : ICRUDRepository<TReadDto, TCreateDto, TUpdateDto> 
    where TEntity : BaseEntity 
    where TReadDto : BaseReadDTO
    where TCreateDto : BaseCreateDTO
    where TUpdateDto : BaseUpdateDTO
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

    public async Task<IEnumerable<TReadDto>> GetAllAsync()
    {
        var entities = await GetAll().ToListAsync();
        return _mapper.Map<IEnumerable<TReadDto>>(entities);
    }

    protected IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>();
    }

    public async Task<Guid> AddAsync(TCreateDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<Guid> UpdateAsync(TUpdateDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetAll().FirstAsync(e => e.Id == id);
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
