namespace Core.Interfaces.Repositories;
public interface ICRUDRepository<TEntity>
{
    IQueryable<TEntity> GetAll();
    Task<TEntity> AddAsync(TEntity entity, bool isSaved = true);
    Task<TEntity> UpdateAsync(TEntity entity, bool isSaved = true);
    Task DeleteAsync(Guid id, bool isSaved = true);

    Task SaveChangesAsync();
}
