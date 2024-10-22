using Core.DTOs.TodoTask;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface ITodoTaskService
{
    Task<Result<IEnumerable<TodoTaskReadDTO>>> GetAllAsync();
    Task<Result<TodoTaskReadDTO>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<TodoTaskReadDTO>>> GetByProjectIdAsync(Guid projectId);
    Task<Result<Guid>> CreateTodoTaskAsync(TodoTaskCreateDTO dto);
    Task<Result<Guid>> UpdateTodoTaskAsync(TodoTaskUpdateDTO dto);
    Task<Result<bool>> DeleteTodoTaskAsync(Guid id);
}
