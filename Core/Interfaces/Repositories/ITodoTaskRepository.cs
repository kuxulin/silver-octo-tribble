using Core.DTOs.TodoTask;
using Core.Entities;

namespace Core.Interfaces.Repositories;
public interface ITodoTaskRepository : ICRUDRepository<TodoTask>
{
}
