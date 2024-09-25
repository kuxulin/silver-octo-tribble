using Core.DTOs.TodoTask;

namespace Core.Interfaces.Services;
public interface ITodoTaskService
{
    Task<IEnumerable<TodoTaskReadDTO>> GetAllAsync();
    Task<TodoTaskReadDTO> GetByIdAsync(Guid id);
    Task<TodoTaskReadDTO> GetByTitleAsync(string title);
    Task<Guid> CreateTodoTaskAsync(TodoTaskCreateDTO dto);
    Task<Guid> UpdateTodoTaskAsync(TodoTaskUpdateDTO dto);
    Task DeleteTodoTaskAsync(Guid id);
}
