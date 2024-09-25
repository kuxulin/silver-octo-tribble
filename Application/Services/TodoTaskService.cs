using Core.DTOs.TodoTask;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services;
internal class TodoTaskService : ITodoTaskService
{
    private readonly ITodoTaskRepository _repository;

    public TodoTaskService(ITodoTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TodoTaskReadDTO>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TodoTaskReadDTO> GetByIdAsync(Guid id)
    {
        var todoTasks = await GetAllAsync();
        return todoTasks.FirstOrDefault(t => t.Id == id);
    }

    public async Task<TodoTaskReadDTO> GetByTitleAsync(string title)
    {
        var todoTasks = await GetAllAsync();
        return todoTasks.FirstOrDefault(t => t.Title == title);
    }

    public async Task<Guid> CreateTodoTaskAsync(TodoTaskCreateDTO dto)
    {
        var duplicate = await GetByTitleAsync(dto.Title);

        if (duplicate?.ProjectId == dto.ProjectId)
            throw new Exception("There is already such tusk in the project.");

        var result = await _repository.AddAsync(dto);
        return result;
    }

    public async Task<Guid> UpdateTodoTaskAsync(TodoTaskUpdateDTO dto)
    {
        var task = await GetByIdAsync(dto.Id);

        if (task is null)
            throw new Exception("There is no such tusk in database.");

        var result = await _repository.UpdateAsync(dto);
        return result;
    }

    public async Task DeleteTodoTaskAsync(Guid id)
    {
        var task = await GetByIdAsync(id);

        if (task is null)
            throw new Exception("There is no such tusk in database.");

        await _repository.DeleteAsync(id);
    }
}
