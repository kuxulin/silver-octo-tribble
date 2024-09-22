using Core.DTOs.Base;

namespace Core.DTOs.TodoTask;
public class TodoTaskCreateDTO :BaseCreateDTO
{
    public string Title { get; set; }

    public string Text { get; set; }

    public Guid ProjectId { get; set; }
}
