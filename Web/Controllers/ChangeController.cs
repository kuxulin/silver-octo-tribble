using Core.Constants;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = DefinedPolicy.DefaultPolicy)]
    public class ChangeController : ControllerBase
    {
        private readonly IChangeService _changeService;

        public ChangeController(IChangeService changeService)
        {
            _changeService = changeService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectChanges(Guid projectId)
        {
            var changes = await _changeService.GetChangesByProjectIdAsync(projectId);
            return Ok(changes);
        }
    }
}
