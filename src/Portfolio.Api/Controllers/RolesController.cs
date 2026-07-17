using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Portfolio.Application.Features.Roles.Queries.GetRoles;
using Portfolio.Application.Features.Roles.Commands.CreateRole;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Super Admin,Admin")]
public class RolesController : BaseApiController
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tüm rolleri listele
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "Roles.Read")]
    public async Task<ActionResult<List<RoleDto>>> GetRoles()
    {
        var query = new GetRolesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Yeni rol oluştur (Super Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "Roles.Create")]
    public async Task<ActionResult<Guid>> CreateRole([FromBody] CreateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRoles), new { id = result }, result);
    }

    /// <summary>
    /// Rol güncelle (Super Admin only)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Roles.Update")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] object command)
    {
        // Implement UpdateRole handler
        return NotFound("Not implemented yet");
    }

    /// <summary>
    /// Rol sil (Super Admin only - sistem rolleri hariç)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Roles.Delete")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        // Implement DeleteRole handler
        return NotFound("Not implemented yet");
    }

    /// <summary>
    /// Rol izinlerini yönet (Super Admin only)
    /// </summary>
    [HttpPut("{id:guid}/permissions")]
    [Authorize(Policy = "Roles.Update")]
    public async Task<IActionResult> UpdateRolePermissions(Guid id, [FromBody] object command)
    {
        // Implement permission management
        return NotFound("Not implemented yet");
    }
}
