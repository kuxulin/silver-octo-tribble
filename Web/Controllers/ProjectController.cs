using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.ViewModels; 
using Core.Entities;
using Persistence.Data.Contexts;
using AutoMapper;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public ProjectController(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _context.Projects.ToListAsync();

        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectCreateViewModel model)
    {
        //var project = new Project()
        //{
        //    Name = model.Name,
        //};
        var project = _mapper.Map<Project>(model);
        _context.Add(project);
        await _context.SaveChangesAsync();

        return Ok(project.Id);
    }
}
