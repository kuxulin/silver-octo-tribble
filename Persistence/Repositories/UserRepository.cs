using Core.Entities;
using Core.Interfaces.Repositories;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;

class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public IQueryable<User> GetAll()
    {
        return _context.Users;
    }
}

