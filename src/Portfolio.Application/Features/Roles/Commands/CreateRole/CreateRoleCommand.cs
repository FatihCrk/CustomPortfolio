using MediatR;

namespace Portfolio.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(
    string Name,
    string? Description = null,
    bool IsDefault = false
) : IRequest<Guid>;
