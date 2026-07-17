using MediatR;
using Portfolio.Application.Interfaces.Repositories;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Features.Skills.Commands.CreateSkill;

public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, Guid>
{
    private readonly IRepository<Skill> _repository;

    public CreateSkillCommandHandler(IRepository<Skill> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = new Skill
        {
            Name = request.Name,
            Level = request.Level,
            Category = request.Category,
            Icon = request.Icon,
            Order = request.Order
        };

        await _repository.AddAsync(skill, cancellationToken);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return skill.Id;
    }
}
