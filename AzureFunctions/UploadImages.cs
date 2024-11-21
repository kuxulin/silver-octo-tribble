using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Constants;
using Core.Interfaces.Services;
using Microsoft.Azure.WebJobs;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunctions;

public class UploadImages
{
    private readonly IImageTransformingService _imageTransformingService;

    public UploadImages(IImageTransformingService imageTransformingService)
    {
        _imageTransformingService = imageTransformingService;
    }

    [FunctionName(nameof(UploadImages))]
    public async Task Run(
    [BlobTrigger("%ImagesContainerName%/original/{imageId}", Connection = "StorageAccountConnectionString")] BlobClient originalBlobClient,
    [Blob("%ImagesContainerName%/thumbnail/{imageId}", FileAccess.Write, Connection = "StorageAccountConnectionString")] BlobClient thumbnailBlobClient,
    [Blob("%ImagesContainerName%/profile/{imageId}", FileAccess.Write, Connection = "StorageAccountConnectionString")] BlobClient profileBlobClient)
    {
        var properties = await originalBlobClient.GetPropertiesAsync();
        var contentType = properties.Value.ContentType;

        using var originalStream = new MemoryStream();
        await originalBlobClient.DownloadToAsync(originalStream);
        var originalContent = originalStream.ToArray();

        await UploadContent(thumbnailBlobClient, originalContent, contentType, DefinedImageParameters.ThumbnailWidth, DefinedImageParameters.ThumbnailHeight);
        await UploadContent(profileBlobClient, originalContent, contentType, DefinedImageParameters.ProfileWidth, DefinedImageParameters.ProfileHeight);
    }

    private async Task UploadContent(BlobClient blobClient, byte[] originalContent, string type, int width, int height)
    {
        var content = _imageTransformingService.CompressImage(originalContent, type, width, height);
        using var stream = new MemoryStream(content);
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = type });
    }
}

