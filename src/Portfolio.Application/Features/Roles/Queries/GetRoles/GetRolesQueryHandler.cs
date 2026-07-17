using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Interfaces.Repositories;
using AutoMapper;

namespace Portfolio.Application.Features.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public GetRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAll()
            .Include(r => r.Users)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        var roleDtos = _mapper.Map<List<RoleDto>>(roles);
        
        // Kullanıcı sayılarını hesapla
        foreach (var roleDto in roleDtos)
        {
            var role = roles.FirstOrDefault(r => r.Id == roleDto.Id);
            if (role != null)
                roleDto.UserCount = role.Users?.Count ?? 0;
        }

        return roleDtos;
    }
}
