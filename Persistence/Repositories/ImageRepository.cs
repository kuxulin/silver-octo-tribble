using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;

class ImageRepository : BaseCRUDRepository<ApplicationImage, DatabaseContext>, IImageRepository
{
    public ImageRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }
}

