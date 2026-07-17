using MediatR;

namespace Portfolio.Application.Features.Skills.Queries.GetSkills;

public record GetSkillsQuery(
    string? Category = null
) : IRequest<List<SkillDto>>;

public class SkillDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
}
