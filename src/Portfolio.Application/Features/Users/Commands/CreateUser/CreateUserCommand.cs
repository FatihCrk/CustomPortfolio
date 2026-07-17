using MediatR;

namespace Portfolio.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password,
    Guid RoleId
) : IRequest<Guid>;
