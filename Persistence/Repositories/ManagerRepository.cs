using AutoMapper;
using Core.DTOs.Manager;
using Core.Entities;
using Core.Interfaces.Repositories;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;
internal class ManagerRepository :BaseCRUDRepository<Manager, DatabaseContext>, IManagerRepository
{
    public ManagerRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }

    
}
