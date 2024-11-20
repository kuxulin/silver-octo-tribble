using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;

class ImageRepository : BaseCRUDRepository<ApplicationImage, DatabaseContext>, IImageRepository
{
    public ImageRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public async Task DeleteAsync(IEnumerable<Guid> ids, bool isSaved = true)
    {
        var images = await _context.Images.Where(e => ids.Contains(e.Id)).ToListAsync();
        _context.Images.RemoveRange(images);

        if (isSaved)
            await _context.SaveChangesAsync();
    }
}

