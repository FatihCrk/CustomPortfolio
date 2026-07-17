using MediatR;
using Portfolio.Application.Interfaces.Repositories;
using Portfolio.Domain.Entities.Identity;
using Portfolio.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Kullanıcı adı veya e-posta kontrolü
        var existingUser = await _userRepository.AnyAsync(
            u => u.Username == request.Username || u.Email == request.Email,
            cancellationToken);

        if (existingUser)
            throw new ApiException("Bu kullanıcı adı veya e-posta adresi zaten kullanılıyor.");

        // Rol kontrolü
        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
        if (role == null)
            throw new ApiException("Belirtilen rol bulunamadı.");

        // Şifre hashleme
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // Yeni kullanıcı oluşturma
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            RoleId = request.RoleId,
            IsActive = true,
            EmailConfirmed = false
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
