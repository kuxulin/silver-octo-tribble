using AutoMapper;
using Core.DTOs.ApplicationImage;
using Core.DTOs.Base;
using Core.Entities;
using Core.Interfaces.Repositories;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;

class ImageRepository : BaseCRUDRepository<ApplicationImage, DatabaseContext>, IImageRepository
{
    public ImageRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public async Task ReplaceImage(ApplicationImage oldImage, ApplicationImage newImage)
    {
        if (oldImage is not null)
            _context.Images.Remove(oldImage);

        _context.Images.Add(newImage);
        await _context.SaveChangesAsync();
    }
}

