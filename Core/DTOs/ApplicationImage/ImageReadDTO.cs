using Core.DTOs.Base;

namespace Core.DTOs.ApplicationImage;
public class ImageReadDTO : BaseReadDTO
{
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Type { get; set; } = null!;
}
