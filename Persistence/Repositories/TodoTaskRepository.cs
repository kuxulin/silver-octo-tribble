using AutoMapper;
using Core.DTOs.TodoTask;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence.Repositories;
internal class TodoTaskRepository : BaseCRUDRepository<TodoTask, TodoTaskReadDTO, TodoTaskCreateDTO, TodoTaskUpdateDTO, DatabaseContext>, ITodoTaskRepository
{
    public TodoTaskRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {

    }

    protected new IQueryable<TodoTask> GetAll()
    {
        return base.GetAll().Include(t=> t.Project);
    }

    public async new Task<Guid> UpdateAsync(TodoTaskUpdateDTO dto)
    {
        var todoTask = await GetAll().Where(t => t.Id == dto.Id).FirstAsync();
        todoTask.Title = dto.Title;
        todoTask.Text = dto.Text;
        await _context.SaveChangesAsync();
        return todoTask.Id;
    }
}
