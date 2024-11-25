using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
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
        await originalBlobClient.DeleteIfExistsAsync();
        await thumbnailBlobClient.DeleteIfExistsAsync();
        await profileBlobClient.DeleteIfExistsAsync();
    }
}