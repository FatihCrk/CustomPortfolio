using FluentValidation;

namespace Portfolio.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Rol adı zorunludur.")
            .MinimumLength(2).WithMessage("Rol adı en az 2 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Rol adı en fazla 50 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Açıklama en fazla 200 karakter olabilir.");
    }
}
