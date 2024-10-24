using AutoMapper;
using Core.DTOs.Change;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

internal class ChangeService : IChangeService
{
    private readonly IChangeRepository _changeRepository;
    private readonly IMapper _mapper;

    public ChangeService(IChangeRepository changeRepository, IMapper mapper)
    {
        _changeRepository = changeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ChangeReadDTO>> GetChangesByProjectIdAsync(Guid projectId)
    {
        var changes = await _changeRepository.GetAll().Where(c => c.ProjectId == projectId).ToListAsync();

        return _mapper.Map<IEnumerable<ChangeReadDTO>>(changes);
    }

    public async Task<IEnumerable<ChangeReadDTO>> GetChangesByUserIdAsync(int userId)
    {
        var changes = await _changeRepository.GetAll().Where(c => c.CreatorId == userId).ToListAsync();

        return _mapper.Map<IEnumerable<ChangeReadDTO>>(changes);
    }

    public async Task CreateChangeAsync(ChangeCreateDTO dto)
    {
        var change = _mapper.Map<Change>(dto);
        await _changeRepository.AddAsync(change);
    }
}
