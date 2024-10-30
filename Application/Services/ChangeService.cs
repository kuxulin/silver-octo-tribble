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
    private readonly IUserRepository _userRepository;
    private readonly IUserChangeRepository _userChangeRepository;
    private readonly ITokenService _tokenService;

    public ChangeService(IChangeRepository changeRepository, IProjectRepository projectRepository, IUserChangeRepository userChangeRepository, IUserRepository userRepository,ITokenService tokenService, IMapper mapper)
    {
        _changeRepository = changeRepository;
        _mapper = mapper;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _userChangeRepository = userChangeRepository;
        _tokenService = tokenService;
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
            .Include(c => c.UserChanges)
            .Where(c => c.ActionType == DefinedAction.ChangeStatus
                        && projects.Select(p => p.Id).Contains(c.ProjectId))
            .ToListAsync();

        var userId = await _userRepository.GetAll().Where(u => u.ManagerId == managerId).Select(u => u.Id).FirstAsync();
        changes = await AttachUserChangesAsync(changes, userId);
        return _mapper.Map<IEnumerable<ChangeReadDTO>>(changes, opts => opts.Items["userId"] = userId);
    }

    public async Task<IEnumerable<ChangeReadDTO>> GetChangesByEmployeeIdAsync(Guid employeeId)
    {
        var query = _changeRepository.GetAll().Include(c => c.UserChanges).Where(c => c.Task.EmployeeId == employeeId);
        var result = new List<Change>();

        var lastAssignChangeFlags = await query
            .Where(c => c.ActionType == DefinedAction.Assign)
            .GroupBy(c => c.TaskId, (key, value) => value.OrderByDescending(c => c.CreationDate).First())
            .ToListAsync();

        foreach (var flagChange in lastAssignChangeFlags)
        {
            var taskChanges = await query
                                    .Where(c => c.TaskId == flagChange.TaskId
                                             && c.CreationDate >= flagChange.CreationDate)
                                    .ToListAsync();

            result.AddRange(taskChanges);
        }

        var userId = await _userRepository.GetAll().Where(u => u.EmployeeId == employeeId).Select(u => u.Id).FirstAsync();
        result = await AttachUserChangesAsync(result, userId);
        return _mapper.Map<IEnumerable<ChangeReadDTO>>(result, opts => opts.Items["userId"] = userId);
    }

    private async Task<List<Change>> AttachUserChangesAsync(List<Change> actualChanges, int userId)
    {
        foreach (var change in actualChanges)
        {
            if (!change.UserChanges.Any(uc => uc.UserId == userId))
            {
                var userChange = new UserChange() { ChangeId = change.Id, UserId = userId };
                change.UserChanges.Add(userChange);
                await _userChangeRepository.AddUserChangeAsync(userChange, isSaved: false);
            }
        }

        await _userChangeRepository.SaveChangesAsync();
        return actualChanges;
    }

    public async Task<Guid> CreateChangeAsync(ChangeCreateDTO dto)
    {
        var change = _mapper.Map<Change>(dto);
        change = await _changeRepository.AddAsync(change);
        return change.Id;
    }

    public async Task MakeChangesRead(IEnumerable<Guid> changeIds, string token)
    {
        int userId = int.Parse(_tokenService.GetFieldFromToken(token, DefinedClaim.Id));
        var changes = await _changeRepository.GetAll().Include(c => c.UserChanges).Where(c => changeIds.Any(id => id == c.Id)).ToListAsync();

        foreach (var change in changes)
        {
            change.UserChanges.Where(uc => uc.UserId == userId).First().IsRead = true;
            await _changeRepository.UpdateAsync(change,isSaved:false);
        }

        await _changeRepository.SaveChangesAsync();
    }
}
