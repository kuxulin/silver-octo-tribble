namespace Core.Interfaces.Services;

public interface IImageTransformingService
{
    byte[] CompressImage(byte[] content, string mimeType, int width, int height);
}

