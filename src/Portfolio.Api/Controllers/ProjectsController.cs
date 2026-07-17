using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entities;
using System.Threading.Tasks;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IRepository<Project> _repository;

    public ProjectsController(IRepository<Project> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _repository.GetAllAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost]
    [Authorize(Policy = "RequireEditorRole")]
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _repository.AddAsync(project);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "RequireEditorRole")]
    public async Task<IActionResult> Update(int id, [FromBody] Project project)
    {
        if (id != project.Id) return BadRequest();
        await _repository.UpdateAsync(project);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null) return NotFound();
        await _repository.DeleteAsync(project);
        return NoContent();
    }
}
