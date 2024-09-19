using Microsoft.AspNetCore.Mvc;
using Contexts;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels;
using Core.Entities;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly DatabaseContext _context;

    public ProjectController(DatabaseContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _context.Projects.ToListAsync();

        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectCreateVewModel model)
    {
        var project = new Project()
        {
            Name = model.Name,

        };

        _context.Add(project);
        await _context.SaveChangesAsync();

        return Ok(project.Id);
    }
}
