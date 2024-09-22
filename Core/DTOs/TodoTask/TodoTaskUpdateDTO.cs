using Core.DTOs.Base;

namespace Core.DTOs.TodoTask;
public class TodoTaskUpdateDTO :BaseUpdateDTO
{
    public string Title { get; set; }
    public string Text { get; set; }
}
