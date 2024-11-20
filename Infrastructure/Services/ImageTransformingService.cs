using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Core.Interfaces.Services;

namespace Infrastructure.Services;

public class ImageTransformingService : IImageTransformingService
{
    public byte[] CompressImage(byte[] content, string mimeType, int width, int height)
    {
        using var inputStream = new MemoryStream(content);
        using var outputStream = new MemoryStream();
        var image = Image.Load(inputStream);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(width, height)
        }));

        var encoder = GetEncoder(mimeType);
        image.Save(outputStream, encoder);
        return outputStream.ToArray();
    }

    private IImageEncoder GetEncoder(string mimeType)
    {
        return mimeType switch
        {
            "image/jpeg" or "image/jpg" => new JpegEncoder(),
            "image/png" => new PngEncoder(),
            "image/bmp" => new BmpEncoder(),
            "image/gif" => new GifEncoder(),
            _ => new JpegEncoder()
        };
    }
}

