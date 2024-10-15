namespace Core.Interfaces.Repositories;
public interface ICRUDRepository<TEntity>
{
    IQueryable<TEntity> GetAll();
    Task<TEntity> AddAsync(TEntity dto);
    Task<TEntity> UpdateAsync(TEntity dto);
    Task DeleteAsync(Guid id);
}
