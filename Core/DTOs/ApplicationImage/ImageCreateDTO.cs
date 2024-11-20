using Core.DTOs.Base;

namespace Core.DTOs.ApplicationImage;
public class ImageCreateDTO :BaseCreateDTO
{
    public int? UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Content { get; set; } = null!;
}
