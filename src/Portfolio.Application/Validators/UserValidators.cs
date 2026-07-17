using FluentValidation;
using Portfolio.Shared.DTOs.Users;

namespace Portfolio.Application.Validators;

public class UserCreateValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad gereklidir")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad gereklidir")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı gereklidir")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta gereklidir")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
            .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre gereklidir")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır")
            .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir")
            .Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir")
            .Matches(@"[0-9]").WithMessage("Şifre en az bir rakam içermelidir")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir");

        RuleFor(x => x.RoleIds)
            .NotEmpty().WithMessage("En az bir rol seçilmelidir")
            .Must(x => x.Count <= 5).WithMessage("En fazla 5 rol seçilebilir");
    }
}

public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad gereklidir")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad gereklidir")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı gereklidir")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta gereklidir")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
            .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir");

        RuleFor(x => x.RoleIds)
            .NotEmpty().WithMessage("En az bir rol seçilmelidir")
            .Must(x => x.Count <= 5).WithMessage("En fazla 5 rol seçilebilir");
    }
}

public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordDto>
{
    public ChangeUserPasswordValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre gereklidir")
            .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır")
            .Matches(@"[A-Z]").WithMessage("Yeni şifre en az bir büyük harf içermelidir")
            .Matches(@"[a-z]").WithMessage("Yeni şifre en az bir küçük harf içermelidir")
            .Matches(@"[0-9]").WithMessage("Yeni şifre en az bir rakam içermelidir")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Yeni şifre en az bir özel karakter içermelidir");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Şifre tekrarı gereklidir")
            .Equal(x => x.NewPassword).WithMessage("Şifreler eşleşmiyor");
    }
}
