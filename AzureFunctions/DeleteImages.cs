using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Constants;
using Core.Interfaces.Services;
using Microsoft.Azure.WebJobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunctions;

public class DeleteImages
{
    [FunctionName(nameof(DeleteImages))]
    public async Task Run(
    [BlobTrigger("%ImagesContainerName%/{imageId}__delete", Connection = "StorageAccountConnectionString")] BlobClient originalBlobClient,
    [Blob("%ImagesContainerName%/thumbnail/{imageId}", FileAccess.Write, Connection = "StorageAccountConnectionString")] BlobClient thumbnailBlobClient,
    [Blob("%ImagesContainerName%/profile/{imageId}", FileAccess.Write, Connection = "StorageAccountConnectionString")] BlobClient profileBlobClient)
    {
        await originalBlobClient.DeleteAsync();
        await thumbnailBlobClient.DeleteAsync();
        await profileBlobClient.DeleteAsync();
    }
}