using AutoMapper;
using Core.Constants;
using Core.DTOs.Project;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Application.Services;

class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITodoTaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository repository, IManagerRepository managerRepository, IEmployeeRepository employeeRepository, ITodoTaskRepository taskRepository, IMapper mapper)
    {
        _projectRepository = repository;
        _managerRepository = managerRepository;
        _employeeRepository = employeeRepository;
        _taskRepository = taskRepository;
        _mapper = mapper;

    }

    public async Task<Result<IEnumerable<ProjectReadDTO>>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAll().ToListAsync();
        return _mapper.Map<List<ProjectReadDTO>>(projects);
    }

    public async Task<Result<ProjectReadDTO>> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetAll()
            .Include(p => p.Managers)
            .ThenInclude(m => m.User)
            .ThenInclude(u => u.Image)
            .Include(p => p.Employees!)
            .ThenInclude(e => e.User)
            .ThenInclude(u => u.Image)
            .Include(p => p.ToDoTasks)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return DefinedError.AbsentElement;

        return _mapper.Map<ProjectReadDTO>(project);
    }

    public async Task<Result<IEnumerable<ProjectReadDTO>>> GetProjectsByManagerIdAsync(Guid id)
    {
        var projects = await _projectRepository.GetProjectsByManagerId(id).ToListAsync();
        return _mapper.Map<List<ProjectReadDTO>>(projects);
    }

    public async Task<Result<IEnumerable<ProjectReadDTO>>> GetProjectsByEmployeeIdAsync(Guid id)
    {
        var projects = await _projectRepository.GetProjectsByEmployeeId(id).ToListAsync();
        return _mapper.Map<List<ProjectReadDTO>>(projects);
    }

    public async Task<Result<ProjectReadDTO>> CreateProjectAsync(ProjectCreateDTO dto)
    {
        var duplicate = await _projectRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.Name);

        if (duplicate is not null)
            return DefinedError.DuplicateEntity;

        var areSomeManagerIdsInvalid = dto.ManagerIds.Except(_managerRepository.GetAll().Select(m => m.Id)).Any();
        var areSomeEmployeeIdsInvalid = dto.EmployeeIds?.Except(_employeeRepository.GetAll().Select(e => e.Id)).Any() ?? false;

        if (areSomeManagerIdsInvalid || areSomeEmployeeIdsInvalid)
            return DefinedError.AbsentElement;

        var project = _mapper.Map<Project>(dto);
        var id = (await _projectRepository.AddAsync(project)).Id;
        return await GetByIdAsync(id);
    }

    public async Task<Result<ProjectReadDTO>> UpdateProjectAsync(ProjectUpdateDTO dto)
    {
        var project = await _projectRepository.GetAll()
            .Include(p => p.Managers)
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Id == dto.Id);

        if (project is null)
            return DefinedError.AbsentElement;

        var areSomeEmployeeIdsInvalid = dto.EmployeeIds?.Except(_employeeRepository.GetAll().Select(e => e.Id)).Any() ?? false;
        var areSomeManagerIdsInvalid = dto.ManagerIds?.Except(_managerRepository.GetAll().Select(m => m.Id)).Any() ?? false;

        if (areSomeEmployeeIdsInvalid || areSomeManagerIdsInvalid)
            return DefinedError.InvalidElement;

        if (dto.Name is not null)
            project.Name = dto.Name;

        if (dto.Description is not null)
            project.Description = dto.Description;

        if (dto.EmployeeIds is not null)
        {
            await UpdateProjectEmployeesAsync(project, dto);
        }

        if (dto.ManagerIds is not null)
            project.Managers = dto.ManagerIds.Select(id => new Manager { Id = id }).ToList();

        project = await _projectRepository.UpdateAsync(project);
        var result = _mapper.Map<ProjectReadDTO>(project);
        return result;
    }

    private async Task UpdateProjectEmployeesAsync(Project project, ProjectUpdateDTO dto)
    {
        var existingEmployeeIds = project.Employees?.Select(e => e.Id) ?? [];
        var employeeToRemoveIds = existingEmployeeIds.Except(dto.EmployeeIds!);
        var employeeToAddIds = dto.EmployeeIds!.Except(existingEmployeeIds);

        foreach (var employeeToRemoveId in employeeToRemoveIds)
        {
            project.Employees!.Remove(project.Employees.FirstOrDefault(e => e.Id == employeeToRemoveId)!);
            var tasks = await _taskRepository.GetAll().Where(t => t.EmployeeId == employeeToRemoveId).ToListAsync();

            foreach (var task in tasks)
            {
                task.Employee = null;
                await _taskRepository.UpdateAsync(task, false);
            }
        }

        if (employeeToAddIds.Any())
        {
            var employeesToAdd = await _employeeRepository.GetAll().Where(e => employeeToAddIds.Contains(e.Id)).ToListAsync();

            foreach (var employeeToAdd in employeesToAdd)
                project.Employees!.Add(employeeToAdd);
        }
    }

    public async Task<Result<bool>> DeleteProjectAsync(Guid id)
    {
        var project = await _projectRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id); ;

        if (project is null)
            return DefinedError.AbsentElement;

        await _projectRepository.DeleteAsync(id);
        return true;
    }

    public async Task<Result<ProjectReadDTO>> GetByNameAsync(string name)
    {
        var project = await _projectRepository.GetAll().FirstOrDefaultAsync(p => p.Name == name);

        if (project is null)
            return DefinedError.AbsentElement;

        var result = _mapper.Map<ProjectReadDTO>(project);
        return result;
    }
}

