namespace Core.Interfaces.Repositories;
public interface ICRUDRepository<TReadDto, TCreateDto, TUpdateDto> where TReadDto : class where TCreateDto : class where TUpdateDto : class
{
    Task<IEnumerable<TReadDto>> GetAllAsync();
    Task<Guid> AddAsync(TCreateDto dto);
    Task<Guid> UpdateAsync(TUpdateDto dto);
    Task DeleteAsync(Guid id);
}
