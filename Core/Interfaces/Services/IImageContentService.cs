using Core.Entities;
using Core.ResultPattern;

namespace Core.Interfaces.Services;

public interface IImageContentService
{
    Task<Result<Guid>> UploadContentAsync(ApplicationImage image);
    Task<Result<byte[]>> GetOriginalContentAsync(Guid id);
    Task<Result<string>> GetThumbnailAsync(Guid id);
    Task<Result<string>> GetProfileAsync(Guid id);
    Task<Result<Guid>> DeleteContentAsync(Guid imageId);
}

