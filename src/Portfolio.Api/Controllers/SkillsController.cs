using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Portfolio.Application.Features.Skills.Queries.GetSkills;
using Portfolio.Application.Features.Skills.Commands.CreateSkill;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : BaseApiController
{
    private readonly IMediator _mediator;

    public SkillsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Yetenekleri listele
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<SkillDto>>> GetSkills([FromQuery] string? category = null)
    {
        var query = new GetSkillsQuery(category);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Yeni yetenek ekle (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Super Admin,Admin,Editor")]
    public async Task<ActionResult<Guid>> CreateSkill([FromBody] CreateSkillCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSkills), new { id = result }, result);
    }

    /// <summary>
    /// Yetenek güncelle (Admin only)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Super Admin,Admin,Editor")]
    public async Task<IActionResult> UpdateSkill(Guid id, [FromBody] object command)
    {
        // Implement UpdateSkill handler
        return NotFound("Not implemented yet");
    }

    /// <summary>
    /// Yetenek sil (Admin only)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Super Admin,Admin")]
    public async Task<IActionResult> DeleteSkill(Guid id)
    {
        // Implement DeleteSkill handler
        return NotFound("Not implemented yet");
    }
}
