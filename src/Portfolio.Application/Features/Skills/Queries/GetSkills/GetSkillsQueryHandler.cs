using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Interfaces.Repositories;
using AutoMapper;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Features.Skills.Queries.GetSkills;

public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, List<SkillDto>>
{
    private readonly IRepository<Skill> _repository;
    private readonly IMapper _mapper;

    public GetSkillsQueryHandler(IRepository<Skill> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<SkillDto>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetAll().AsQueryable();

        // Kategori filtresi
        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            query = query.Where(s => s.Category == request.Category);
        }

        var skills = await query
            .OrderBy(s => s.Order)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<SkillDto>>(skills);
    }
}
