using AutoMapper;
using Core.Constants;
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
    private readonly IProjectRepository _projectRepository;

    public ChangeService(IChangeRepository changeRepository, IProjectRepository projectRepository, IMapper mapper)
    {
        _changeRepository = changeRepository;
        _mapper = mapper;
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ChangeReadDTO>> GetChangesByProjectIdAsync(Guid projectId)
    {
        var changes = await _changeRepository.GetAll().Where(c => c.ProjectId == projectId).ToListAsync();

        return _mapper.Map<IEnumerable<ChangeReadDTO>>(changes);
    }

    public async Task<IEnumerable<ChangeReadDTO>> GetChangesByManagerIdAsync(Guid managerId)
    {
        var projects = await _projectRepository.GetProjectsByManagerId(managerId).ToListAsync();
        var changes = await _changeRepository.GetAll().Where(c => c.ActionType == DefinedAction.ChangeStatus && projects.Select(p => p.Id).Contains(c.ProjectId)).ToListAsync();
        return _mapper.Map<IEnumerable<ChangeReadDTO>>(changes);
    }

    public async Task<IEnumerable<ChangeReadDTO>> GetChangesByEmployeeIdAsync(Guid employeeId)
    {
        var projects = await _projectRepository.GetProjectsByEmployeeId(employeeId).ToListAsync();
        var changes = await _changeRepository.GetAll().Where(c => c.Task.EmployeeId == employeeId && projects.Select(p => p.Id).Contains(c.ProjectId)).ToListAsync();
        return _mapper.Map<IEnumerable<ChangeReadDTO>>(changes);
    }

    public async Task CreateChangeAsync(ChangeCreateDTO dto)
    {
        var change = _mapper.Map<Change>(dto);
        await _changeRepository.AddAsync(change);
    }
}
