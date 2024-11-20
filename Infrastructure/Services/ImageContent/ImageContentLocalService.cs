using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services.ImageContent;
internal class ImageContentLocalService : IImageContentService
{
    private readonly IImageRepository _imageRepository;
    private readonly IImageTransformingService _imageTransformingService;

    public ImageContentLocalService(IImageRepository imageRepository,IImageTransformingService imageTransformingService)
    {
        _imageRepository = imageRepository;
        _imageTransformingService = imageTransformingService;
    }

    public async Task<Result<Guid>> UploadContentAsync(ApplicationImage image)
    {
        return await Task.Run(() => image.Id);
    }

    public async Task<Result<byte[]>> GetOriginalContentAsync(Guid id)
    {
        var image = await _imageRepository.GetAll().FirstAsync(i => i.Id == id);

        if (image.Content is null)
            return DefinedError.AbsentElement;

        return image.Content;
    }

    public async Task<Result<string>> GetThumbnailAsync(Guid id)
    {
        var image = await _imageRepository.GetAll().FirstAsync(i => i.Id == id);

        if (image.Content is null)
            return DefinedError.AbsentElement;

        var byteContent = _imageTransformingService.CompressImage(image.Content, image.Type, DefinedImageParameters.ThumbnailWidth, DefinedImageParameters.ThumbnailHeight);
        return Convert.ToBase64String(byteContent);
    }

    public async Task<Result<string>> GetProfileAsync(Guid id)
    {
        var image = await _imageRepository.GetAll().FirstAsync(i => i.Id == id);

        if (image.Content is null)
            return DefinedError.AbsentElement;

        var byteContent = _imageTransformingService.CompressImage(image.Content, image.Type, DefinedImageParameters.ProfileWidth, DefinedImageParameters.ProfileHeight);
        return Convert.ToBase64String(byteContent);
    }

    public async Task<Result<Guid>> DeleteContentAsync(Guid imageId)
    {
        var image = await _imageRepository.GetAll().FirstOrDefaultAsync(i => i.Id == imageId);

        if (image is not null)
        {
            image.Content = [];
            await _imageRepository.UpdateAsync(image);
        }

        return imageId;
    }
}
