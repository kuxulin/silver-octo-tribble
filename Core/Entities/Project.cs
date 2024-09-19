namespace Core.Entities;

public partial class Project : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }
}
