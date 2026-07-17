using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Portfolio.Application.Features.Users.Queries.GetUsers;
using Portfolio.Application.Features.Users.Commands.CreateUser;
using Portfolio.Application.DTOs;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Super Admin,Admin")]
public class UsersController : BaseApiController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Kullanıcıları listele (sayfalama, arama, filtreleme destekli)
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "Users.Read")]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] Guid? roleId = null,
        [FromQuery] bool? isActive = null)
    {
        var query = new GetUsersQuery(page, pageSize, searchTerm, roleId, isActive);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Yeni kullanıcı oluştur
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "Users.Create")]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUsers), new { id = result }, result);
    }

    /// <summary>
    /// Kullanıcı detayını getir
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "Users.Read")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        // Implement GetUsersById query handler
        return NotFound("Not implemented yet");
    }

    /// <summary>
    /// Kullanıcı güncelle
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Users.Update")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] object command)
    {
        // Implement UpdateUser command handler
        return NotFound("Not implemented yet");
    }

    /// <summary>
    /// Kullanıcı sil (soft delete)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Users.Delete")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        // Implement DeleteUser command handler
        return NotFound("Not implemented yet");
    }

    /// <summary>
    /// Kullanıcı durumunu değiştir (aktif/pasif)
    /// </summary>
    [HttpPatch("{id:guid}/toggle-status")]
    [Authorize(Policy = "Users.Update")]
    public async Task<IActionResult> ToggleUserStatus(Guid id)
    {
        // Implement toggle status command
        return NotFound("Not implemented yet");
    }

    /// <summary>
    /// Kullanıcı şifresini sıfırla
    /// </summary>
    [HttpPost("{id:guid}/reset-password")]
    [Authorize(Policy = "Users.Update")]
    public async Task<IActionResult> ResetPassword(Guid id, [FromBody] object command)
    {
        // Implement reset password command
        return NotFound("Not implemented yet");
    }
}
