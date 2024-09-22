using Core.DTOs.TodoTask;

namespace Core.Interfaces.Repositories;
public interface ITodoTaskRepository : ICRUDRepository<TodoTaskReadDTO,TodoTaskCreateDTO,TodoTaskUpdateDTO>
{
}
