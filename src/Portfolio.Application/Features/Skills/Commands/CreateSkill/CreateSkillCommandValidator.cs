using FluentValidation;

namespace Portfolio.Application.Features.Skills.Commands.CreateSkill;

public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Yetenek adı zorunludur.")
            .MinimumLength(2).WithMessage("Yetenek adı en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Yetenek adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Level)
            .InclusiveBetween(1, 100).WithMessage("Seviye 1-100 arasında olmalıdır.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori zorunludur.")
            .MaximumLength(50).WithMessage("Kategori en fazla 50 karakter olabilir.");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Sıralama negatif olamaz.");
    }
}
