using Core.DTOs.Base;

namespace Core.DTOs.ApplicationImage;
public class ApplicationImageReadDTO : BaseReadDTO
{
    public string Name { get; set; }
    public string Content { get; set; }
}
