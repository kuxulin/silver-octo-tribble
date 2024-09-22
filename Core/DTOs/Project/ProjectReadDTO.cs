using Core.DTOs.Base;
using Core.DTOs.TodoTask;

namespace Core.DTOs.Project;
public class ProjectReadDTO :BaseReadDTO
{
    public string Name { get; set; }
    public IEnumerable<TodoTaskReadDTO> Tasks { get; set; }
    //todo list of managers, employees
}
