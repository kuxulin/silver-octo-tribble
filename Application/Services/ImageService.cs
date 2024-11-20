using AutoMapper;
using Core.Constants;
using Core.DTOs.ApplicationImage;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
internal class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IImageContentService _imageContentService;
    private readonly IMapper _mapper;

    public ImageService(IImageRepository imageRepository, IImageContentService imageContentService, IMapper mapper)
    {
        _imageRepository = imageRepository;
        _imageContentService = imageContentService;
        _mapper = mapper;
    }

    public async Task<Result<ImageReadDTO>> GetThumbnailAsync(Guid imageId)
    {
        return await GetImage(imageId, _imageContentService.GetThumbnailAsync);
    }

    public async Task<Result<ImageReadDTO>> GetProfileAsync(Guid imageId)
    {
        return await GetImage(imageId, _imageContentService.GetProfileAsync);
    }

    private async Task<Result<ImageReadDTO>> GetImage(Guid imageId, Func<Guid, Task<Result<string>>> fetchContentFunc)
    {
        var image = await _imageRepository.GetAll().FirstOrDefaultAsync(i => i.Id == imageId);
        var imageContentResult = await fetchContentFunc(imageId);

        if (image is null || !imageContentResult.IsSuccess)
            return imageContentResult.Error ?? DefinedError.AbsentElement;

        var dto = _mapper.Map<ImageReadDTO>(image);
        dto.Content = imageContentResult.Value!;
        return dto;
    }

    public async Task<Result<Guid>> AddImageAsync(ImageCreateDTO dto)
    {
        var image = _mapper.Map<ApplicationImage>(dto);
        var contentResult = await _imageContentService.UploadContentAsync(image);

        if (!contentResult.IsSuccess)
            return contentResult;

        try
        {
            await _imageRepository.AddAsync(image);
        }
        catch
        {
            await _imageContentService.DeleteContentAsync(image.Id);
        }

        return image.Id;
    }

    public async Task<Result<bool>> AreEqual(ImageCreateDTO dto, Guid oldImageId)
    {
        var oldImage = await _imageRepository.GetAll().FirstOrDefaultAsync(i => i.Id == oldImageId);
        var newImage = _mapper.Map<ApplicationImage>(dto);

        if (oldImage is null)
            return DefinedError.AbsentElement;

        if (oldImage.Name != dto.Name)
            return false;

        if (oldImage.Content is not null)
        {
            if (oldImage.Content == newImage.Content)
                return true;
            else return false;
        }

        var imageContentResult = await _imageContentService.GetOriginalContentAsync(oldImageId);

        if (!imageContentResult.IsSuccess)
            return imageContentResult.Error!;

        if (imageContentResult.Value! == newImage.Content)
            return true;

        return false;
    }

    public async Task<IEnumerable<Guid>> DeleteImagesAsync(IEnumerable<Guid> ids)
    {
        await _imageRepository.DeleteAsync(ids);

        foreach (var id in ids)
            await _imageContentService.DeleteContentAsync(id);

        return ids;
    }
}
