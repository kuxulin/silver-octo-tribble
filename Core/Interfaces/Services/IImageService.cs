using Core.DTOs.ApplicationImage;
using Core.ResultPattern;

namespace Core.Interfaces.Services;
public interface IImageService
{
    Task<Result<ImageReadDTO>> GetThumbnailAsync(Guid imageId);
    Task<Result<ImageReadDTO>> GetProfileAsync(Guid imageId);
    Task<Result<bool>> AreEqual(ImageCreateDTO dto, Guid oldImageId);
    Task<Result<Guid>> AddImageAsync(ImageCreateDTO dto);
    Task<IEnumerable<Guid>> DeleteImagesAsync(IEnumerable<Guid> ids);
}
