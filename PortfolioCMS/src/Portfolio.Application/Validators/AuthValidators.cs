using FluentValidation;
using Portfolio.Application.DTOs;

namespace Portfolio.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("Kullanıcı adı veya e-posta gereklidir.")
            .MaximumLength(100).WithMessage("Kullanıcı adı veya e-posta çok uzun.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre gereklidir.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
            
        RuleFor(x => x.CaptchaToken)
            .NotEmpty().WithMessage("Güvenlik doğrulaması gereklidir.");
    }
}

public class SetupAdminRequestValidator : AbstractValidator<SetupAdminRequest>
{
    public SetupAdminRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad Soyad gereklidir.")
            .MinimumLength(3).WithMessage("Ad Soyad en az 3 karakter olmalıdır.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı gereklidir.")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta gereklidir.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre gereklidir.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.");
                
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor.");
    }
}

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Proje başlığı gereklidir.")
            .MaximumLength(150).WithMessage("Başlık çok uzun.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama gereklidir.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Proje görseli gereklidir.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
            .WithMessage("Geçerli bir resim URL'si giriniz.");
            
        RuleForEach(x => x.Tags)
            .MaximumLength(50).WithMessage("Etiketler çok uzun olamaz.");
    }
}

public class ContactMessageValidator : AbstractValidator<CreateContactMessageDto>
{
    public ContactMessageValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Adınız gereklidir.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta gereklidir.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mesaj alanı boş olamaz.")
            .MinimumLength(10).WithMessage("Mesaj en az 10 karakter olmalıdır.");
            
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Konu gereklidir.");
    }
}
