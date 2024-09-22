using System;

namespace Core.Entities;
public class BaseEntity
{
    public Guid Id { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public DateTime CreationDate { get; set; }
}
