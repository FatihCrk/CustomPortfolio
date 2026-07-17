using MediatR;

namespace Portfolio.Application.Features.Skills.Commands.CreateSkill;

public record CreateSkillCommand(
    string Name,
    int Level,
    string Category,
    string? Icon = null,
    int Order = 0
) : IRequest<Guid>;
