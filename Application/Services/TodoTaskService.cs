﻿using AutoMapper;
using Core.Constants;
using Core.DTOs.TodoTask;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application.Services;
internal class TodoTaskService : ITodoTaskService
{
    private readonly ITodoTaskRepository _todoTaskRepository;
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProjectRepository _projectRepository;

    public TodoTaskService(ITodoTaskRepository todoTaskRepository, IEmployeeRepository employeeRepository, IProjectRepository projectRepository, IMapper mapper)
    {
        _todoTaskRepository = todoTaskRepository;
        _mapper = mapper;
        _employeeRepository = employeeRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result<IEnumerable<TodoTaskReadDTO>>> GetAllAsync()
    {
        var tasks = await _todoTaskRepository.GetAll().ToListAsync();
        return _mapper.Map<List<TodoTaskReadDTO>>(tasks);
    }

    public async Task<Result<TodoTaskReadDTO>> GetByIdAsync(Guid id)
    {
        var task = await _todoTaskRepository.GetAll().FirstOrDefaultAsync(t => t.Id == id);
        return _mapper.Map<TodoTaskReadDTO>(task);
    }

    public async Task<Result<IEnumerable<TodoTaskReadDTO>>> GetByProjectIdAsync(Guid projectId)
    {
        var tasks = await _todoTaskRepository.GetAll().Where(t => t.ProjectId == projectId).ToListAsync();
        return _mapper.Map<List<TodoTaskReadDTO>>(tasks);
    }

    public async Task<Result<TodoTaskReadDTO>> CreateTodoTaskAsync(TodoTaskCreateDTO dto)
    {
        var duplicate = await _todoTaskRepository.GetAll().FirstOrDefaultAsync(t => t.Title == dto.Title);

        if (duplicate?.ProjectId == dto.ProjectId)
            return DefinedError.DuplicateEntity;

        var employee = await _employeeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == dto.EmployeeId);
        var project = await _projectRepository.GetAll().FirstOrDefaultAsync(e => e.Id == dto.ProjectId);

        if (project is null)
            return DefinedError.AbsentElement;

        var task = _mapper.Map<TodoTask>(dto);
        task.Employee = employee;
        task.Project = project;
        var result = await _todoTaskRepository.AddAsync(task);
        return _mapper.Map<TodoTaskReadDTO>(result);
    }

    public async Task<Result<TodoTaskReadDTO>> UpdateTodoTaskAsync(TodoTaskUpdateDTO dto)
    {
        var task = await _todoTaskRepository.GetAll().FirstOrDefaultAsync(t => t.Id == dto.Id);

        if (task is null)
            return DefinedError.AbsentElement;

        if (dto.Title is not null)
            task.Title = dto.Title;

        if (dto.Text is not null)
            task.Text = dto.Text;

        var result = await _todoTaskRepository.UpdateAsync(task);
        return _mapper.Map<TodoTaskReadDTO>(result);
    }

    public async Task<Result<TodoTaskReadDTO>> ChangeTaskStatusAsync(Guid taskId, AvailableTaskStatus status)
    {
        var task = await _todoTaskRepository.GetAll().FirstOrDefaultAsync(t => t.Id == taskId);

        if (task is null)
            return DefinedError.AbsentElement;

        if (!Enum.IsDefined(typeof(AvailableTaskStatus), status))
            return DefinedError.InvalidElement;

        var todoTaskStatus = new TodoTaskStatus() { Id = ((int)status) };
        task.Status = todoTaskStatus;
        var result = await _todoTaskRepository.UpdateAsync(task);
        return _mapper.Map<TodoTaskReadDTO>(result);
    }

    public async Task<Result<TodoTaskReadDTO>> ChangeTaskEmployeeAsync(Guid taskId, Guid? employeeId)
    {
        var task = await _todoTaskRepository.GetAll().FirstOrDefaultAsync(t => t.Id == taskId);

        if (task is null)
            return DefinedError.AbsentElement;

        if (employeeId is not null && task.Employee?.Id != employeeId)
        {
            var employee = await _employeeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee is null)
                return DefinedError.AbsentElement;

            task.Employee = employee;
        }
        else if (task.Employee is not null && employeeId is null)
        {
            task.Employee = null;
        }

        var result = await _todoTaskRepository.UpdateAsync(task);
        return _mapper.Map<TodoTaskReadDTO>(result);
    }

    public async Task<Result<TodoTaskReadDTO>> DeleteTodoTaskAsync(Guid id)
    {
        var task = await _todoTaskRepository.GetAll().FirstOrDefaultAsync(t => t.Id == id);

        if (task is null)
            return DefinedError.AbsentElement;

        await _todoTaskRepository.DeleteAsync(id);
        return _mapper.Map<TodoTaskReadDTO>(task);
    }
}
