namespace Core.Entities;

public partial class Task : BaseEntity
{ 
    public string Title { get; set; }

    public string Text { get; set; }

    public Guid ProjectId { get; set; }

    public virtual Project Project { get; set; }
}
