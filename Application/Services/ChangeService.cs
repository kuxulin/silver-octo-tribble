using AutoMapper;
using Core.Constants;
using Core.DTOs.Change;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
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

    public async Task<Result<ChangeReadDTO>> GetChangeByIdAsync(Guid id)
    {
        var change = await _changeRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);

        if (change is null)
            return DefinedError.AbsentElement;

        return _mapper.Map<ChangeReadDTO>(change);
    }
    public async Task<IEnumerable<ChangeReadDTO>> GetChangesByManagerIdAsync(Guid managerId)
    {
        var projects = await _projectRepository.GetProjectsByManagerId(managerId).ToListAsync();

        var changes = await _changeRepository
            .GetAll()
            .Where(c => c.ActionType == DefinedAction.ChangeStatus
                        && projects.Select(p => p.Id).Contains(c.ProjectId))
            .ToListAsync();

        return _mapper.Map<IEnumerable<ChangeReadDTO>>(changes);
    }

    public async Task<IEnumerable<ChangeReadDTO>> GetChangesByEmployeeIdAsync(Guid employeeId)
    {
        var query = _changeRepository.GetAll().Where(c => c.Task.EmployeeId == employeeId);
        var result = new List<Change>();

        var lastAssignChangeFlags = await query
            .Where(c => c.ActionType == DefinedAction.Assign)
            .GroupBy(c => c.TaskId, (key,value) => value.OrderByDescending(c => c.CreationDate).First())
            .ToListAsync();
        
        foreach (var flagChange in lastAssignChangeFlags)
        {
            var taskChanges = await query
                                    .Where(c => c.TaskId == flagChange.TaskId 
                                                && c.CreationDate >= flagChange.CreationDate)
                                    .ToListAsync();

            result.AddRange(taskChanges);                        
        }

        return _mapper.Map<IEnumerable<ChangeReadDTO>>(result);
    }

    public async Task<Guid> CreateChangeAsync(ChangeCreateDTO dto)
    {
        var change = _mapper.Map<Change>(dto);
        change = await _changeRepository.AddAsync(change);
        return change.Id;
    }

    public async Task MakeChangeRead(Guid changeId, int userId)
    {
        var change = await _changeRepository.GetAll().Include(c => c.UserChanges).FirstOrDefaultAsync(c => c.Id == changeId);   

        change.UserChanges.Where(uc => uc.UserId == userId).First().IsRead = true;

        await _changeRepository.UpdateAsync(change);
    }
}
