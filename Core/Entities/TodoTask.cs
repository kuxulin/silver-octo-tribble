namespace Core.Entities;

public class TodoTask : BaseEntity
{ 
    public string Title { get; set; }

    public string Text { get; set; }

    public Guid ProjectId { get; set; }

    public Project Project { get; set; }
}
