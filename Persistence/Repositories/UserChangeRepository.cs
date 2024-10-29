using Core.Entities;
using Core.Interfaces.Repositories;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;
internal class UserChangeRepository : IUserChangeRepository
{
    private readonly DatabaseContext _context;

    public UserChangeRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddUserChangeAsync(UserChange userChange,bool isSaved = true)
    {
        _context.UserChanges.Add(userChange);

        if(isSaved) 
            await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
