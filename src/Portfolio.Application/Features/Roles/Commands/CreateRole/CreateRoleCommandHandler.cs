using MediatR;
using Portfolio.Application.Interfaces.Repositories;
using Portfolio.Application.Exceptions;
using Portfolio.Domain.Entities.Identity;

namespace Portfolio.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // Rol adı kontrolü (benzersiz olmalı)
        var existingRole = await _roleRepository.AnyAsync(r => r.Name == request.Name, cancellationToken);
        if (existingRole)
            throw new ApiException("Bu rol adı zaten kullanılıyor.");

        // Yeni rol oluşturma
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description ?? string.Empty,
            IsDefault = request.IsDefault,
            NormalizedName = request.Name.ToUpper()
        };

        await _roleRepository.AddAsync(role, cancellationToken);
        await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return role.Id;
    }
}
