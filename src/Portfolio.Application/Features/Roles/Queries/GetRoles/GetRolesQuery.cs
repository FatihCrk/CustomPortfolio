using MediatR;

namespace Portfolio.Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery() : IRequest<List<RoleDto>>;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public int UserCount { get; set; }
    public DateTime CreatedDate { get; set; }
}
