using Core.DTOs.Base;

namespace Core.DTOs.ApplicationImage;
public class ApplicationImageCreateDTO :BaseCreateDTO
{
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
}
