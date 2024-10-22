using AutoMapper;
using Core.Constants;
using Core.DTOs.Manager;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
internal class ManagerService : IManagerService
{
    private readonly IManagerRepository _repository;
    private readonly IMapper _mapper;

    public ManagerService(IManagerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ManagerReadDTO>>> GetAllAsync()
    {
        var managers = await _repository.GetAll().Include(m => m.User).ToListAsync();
        return _mapper.Map<List<ManagerReadDTO>>(managers);
    }

    public async Task<Result<IEnumerable<ManagerReadDTO>>> GetManagersInProjectAsync(Guid projectId)
    {
        var managers = await _repository.GetAll().Include(m => m.User).Include(m => m.Projects).Where(m => m.Projects.Any(p => p.Id == projectId)).ToListAsync();
        return _mapper.Map<List<ManagerReadDTO>>(managers);
    }

    public async Task<Result<Guid>> CreateManagerAsync(ManagerCreateDTO dto)
    {
        var duplicate = await _repository.GetAll().FirstOrDefaultAsync(m => m.UserId == dto.UserId);

        if (duplicate is not null)
            return DefinedError.DuplicateEntity;

        var manager = _mapper.Map<Manager>(dto);
        var result = await _repository.AddAsync(manager);
        return result.Id;
    }

    public async Task<Result<bool>> DeleteManagerAsync(Guid id)
    {
        var manager = await _repository.GetAll().FirstOrDefaultAsync(m => m.Id == id);

        if (manager is null)
            return DefinedError.AbsentElement;

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<Result<Guid>> UpdateManagerAsync(ManagerUpdateDTO dto)
    {
        var manager = await _repository.GetAll().FirstOrDefaultAsync(m => m.Id == dto.Id);

        if (manager is null)
            return DefinedError.AbsentElement;

        
        var result = await _repository.UpdateAsync(manager);
        return manager.Id;
    }
}
