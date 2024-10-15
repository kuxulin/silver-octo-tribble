using Core.Entities;

namespace Core.Interfaces.Repositories;
public interface IImageRepository : ICRUDRepository<ApplicationImage>
{
    Task ReplaceImage(ApplicationImage oldImage, ApplicationImage newImage);
}
