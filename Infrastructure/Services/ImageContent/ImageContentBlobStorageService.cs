using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Core.Constants;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ResultPattern;
using OneOf;
using System;

namespace Infrastructure.Services.ImageContent;
internal class ImageContentBlobStorageService : IImageContentService
{
    private readonly BlobContainerClient _containerClient;

    public ImageContentBlobStorageService(BlobContainerClient containerClient)
    {
        _containerClient = containerClient;
    }

    public async Task<Result<byte[]>> GetOriginalContentAsync(Guid id)
    {
        var contentResult = await ReadContentAsync($"original/{id}");

        if (!contentResult.IsSuccess)
            return contentResult;

        return contentResult.Value!;
    }

    public async Task<Result<string>> GetThumbnailAsync(Guid id)
    {
        return await Task.Run(() => GetReadUri($"thumbnail/{id}"));
    }

    public async Task<Result<string>> GetProfileAsync(Guid id)
    {
        return await Task.Run(() => GetReadUri($"profile/{id}"));
    }

    private Result<string> GetReadUri(string url)
    {
        var blobClient = _containerClient.GetBlobClient(url);

        if (!blobClient.Exists())
            return DefinedError.AbsentElement;

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerClient.Name,
            BlobName = url,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(1)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        var blobUri = blobClient.GenerateSasUri(sasBuilder);
        return blobUri.ToString();
    }

    private async Task<Result<byte[]>> ReadContentAsync(string url)
    {
        var blobClient = _containerClient.GetBlobClient(url);

        if (!blobClient.Exists())
            return DefinedError.AbsentElement;

        using var stream = new MemoryStream();
        await blobClient.DownloadToAsync(stream);
        return stream.ToArray();
    }

    public async Task<Result<Guid>> UploadContentAsync(ApplicationImage image)
    {
        var blobClient = _containerClient.GetBlobClient($"original/{image.Id}");

        if (image.Content is null)
            return DefinedError.AbsentElement;

        using var stream = new MemoryStream(image.Content);
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.Type });
        image.Content = [];
        return image.Id;
    }

    public async Task<Result<Guid>> DeleteContentAsync(Guid imageId)
    {
        var blobClient = _containerClient.GetBlobClient($"original/{imageId}");

        if (blobClient.Exists())
            await blobClient.DeleteAsync();

        return imageId;
    }
}
