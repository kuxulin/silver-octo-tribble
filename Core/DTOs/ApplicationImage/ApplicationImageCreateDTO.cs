using Core.DTOs.Base;

namespace Core.DTOs.ApplicationImage;
public class ApplicationImageCreateDTO :BaseCreateDTO
{
    public string Name { get; set; }
    public string Content { get; set; }
}
