using Core.DTOs.TodoTask;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface ITodoTaskService
{
    Task<Result<IEnumerable<TodoTaskReadDTO>>> GetAllAsync();
    Task<Result<TodoTaskReadDTO>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<TodoTaskReadDTO>>> GetByProjectIdAsync(Guid projectId);
    Task<Result<TodoTaskReadDTO>> CreateTodoTaskAsync(TodoTaskCreateDTO dto);
    Task<Result<TodoTaskReadDTO>> UpdateTodoTaskAsync(TodoTaskUpdateDTO dto);
    Task<Result<TodoTaskReadDTO>> ChangeTaskEmployeeAsync(Guid taskId, Guid? employeeId);
    Task<Result<TodoTaskReadDTO>> DeleteTodoTaskAsync(Guid id);
}
