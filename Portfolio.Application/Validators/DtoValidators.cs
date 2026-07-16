using FluentValidation;
using Portfolio.Application.DTOs;

namespace Portfolio.Application.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");
    }
}

public class SetupAdminDtoValidator : AbstractValidator<SetupAdminDto>
{
    public SetupAdminDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta boş olamaz")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
            .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır")
            .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]").WithMessage("Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir");
    }
}

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Mevcut şifre boş olamaz");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre boş olamaz")
            .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]").WithMessage("Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword).WithMessage("Şifreler eşleşmiyor");
    }
}

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Proje başlığı boş olamaz")
            .MaximumLength(200).WithMessage("Proje başlığı en fazla 200 karakter olabilir");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama boş olamaz")
            .MaximumLength(5000).WithMessage("Açıklama en fazla 5000 karakter olabilir");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori boş olamaz")
            .MaximumLength(100).WithMessage("Kategori en fazla 100 karakter olabilir");

        RuleFor(x => x.GithubUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.GithubUrl))
            .WithMessage("Geçerli bir GitHub URL'si giriniz");

        RuleFor(x => x.DemoUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.DemoUrl))
            .WithMessage("Geçerli bir demo URL'si giriniz");
    }

    private bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}

public class CreateMessageDtoValidator : AbstractValidator<CreateMessageDto>
{
    public CreateMessageDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad boş olamaz")
            .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta boş olamaz")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
            .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Konu boş olamaz")
            .MaximumLength(200).WithMessage("Konu en fazla 200 karakter olabilir");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Mesaj boş olamaz")
            .MinimumLength(10).WithMessage("Mesaj en az 10 karakter olmalıdır")
            .MaximumLength(5000).WithMessage("Mesaj en fazla 5000 karakter olabilir");
    }
}

public class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Yetenek adı boş olamaz")
            .MaximumLength(100).WithMessage("Yetenek adı en fazla 100 karakter olabilir");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori boş olamaz")
            .MaximumLength(100).WithMessage("Kategori en fazla 100 karakter olabilir");

        RuleFor(x => x.Level)
            .InclusiveBetween(1, 100).WithMessage("Seviye 1-100 arasında olmalıdır");
    }
}

public class CreateExperienceDtoValidator : AbstractValidator<CreateExperienceDto>
{
    public CreateExperienceDtoValidator()
    {
        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("Şirket adı boş olamaz")
            .MaximumLength(200).WithMessage("Şirket adı en fazla 200 karakter olabilir");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Pozisyon boş olamaz")
            .MaximumLength(200).WithMessage("Pozisyon en fazla 200 karakter olabilir");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi boş olamaz");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.EndDate.HasValue && !x.IsCurrent)
            .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama boş olamaz")
            .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir");
    }
}

public class CreateEducationDtoValidator : AbstractValidator<CreateEducationDto>
{
    public CreateEducationDtoValidator()
    {
        RuleFor(x => x.School)
            .NotEmpty().WithMessage("Okul adı boş olamaz")
            .MaximumLength(200).WithMessage("Okul adı en fazla 200 karakter olabilir");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Bölüm adı boş olamaz")
            .MaximumLength(200).WithMessage("Bölüm adı en fazla 200 karakter olabilir");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi boş olamaz");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz");
    }
}

public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
{
    public CreatePostDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık boş olamaz")
            .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("İçerik boş olamaz")
            .MinimumLength(50).WithMessage("İçerik en az 50 karakter olmalıdır")
            .MaximumLength(50000).WithMessage("İçerik en fazla 50000 karakter olabilir");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori boş olamaz")
            .MaximumLength(100).WithMessage("Kategori en fazla 100 karakter olabilir");

        RuleFor(x => x.ShortDescription)
            .MaximumLength(500).WithMessage("Kısa açıklama en fazla 500 karakter olabilir");
    }
}

public class CreateReferenceDtoValidator : AbstractValidator<CreateReferenceDto>
{
    public CreateReferenceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad boş olamaz")
            .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Ünvan boş olamaz")
            .MaximumLength(100).WithMessage("Ünvan en fazla 100 karakter olabilir");

        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("Şirket boş olamaz")
            .MaximumLength(100).WithMessage("Şirket en fazla 100 karakter olabilir");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Yorum boş olamaz")
            .MaximumLength(1000).WithMessage("Yorum en fazla 1000 karakter olabilir");
    }
}

public class CreateCertificateDtoValidator : AbstractValidator<CreateCertificateDto>
{
    public CreateCertificateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Sertifika başlığı boş olamaz")
            .MaximumLength(200).WithMessage("Sertifika başlığı en fazla 200 karakter olabilir");

        RuleFor(x => x.IssuedBy)
            .NotEmpty().WithMessage("Veren kurum boş olamaz")
            .MaximumLength(200).WithMessage("Veren kurum en fazla 200 karakter olabilir");
    }
}

public class CreateServiceItemDtoValidator : AbstractValidator<CreateServiceItemDto>
{
    public CreateServiceItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Hizmet başlığı boş olamaz")
            .MaximumLength(100).WithMessage("Hizmet başlığı en fazla 100 karakter olabilir");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama boş olamaz")
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");
    }
}

public class CreateSocialMediaDtoValidator : AbstractValidator<CreateSocialMediaDto>
{
    public CreateSocialMediaDtoValidator()
    {
        RuleFor(x => x.Platform)
            .NotEmpty().WithMessage("Platform adı boş olamaz")
            .MaximumLength(50).WithMessage("Platform adı en fazla 50 karakter olabilir");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL boş olamaz")
            .Must(BeValidUrl).WithMessage("Geçerli bir URL giriniz")
            .MaximumLength(500).WithMessage("URL en fazla 500 karakter olabilir");
    }

    private bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}

public class UpdateHeroDtoValidator : AbstractValidator<UpdateHeroDto>
{
    public UpdateHeroDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Ana başlık boş olamaz")
            .MaximumLength(100).WithMessage("Ana başlık en fazla 100 karakter olabilir");

        RuleFor(x => x.Subtitle)
            .NotEmpty().WithMessage("Alt başlık boş olamaz")
            .MaximumLength(200).WithMessage("Alt başlık en fazla 200 karakter olabilir");

        RuleFor(x => x.StatsYearsExperience)
            .GreaterThanOrEqualTo(0).WithMessage("Deneyim yılı negatif olamaz");

        RuleFor(x => x.StatsProjectsCompleted)
            .GreaterThanOrEqualTo(0).WithMessage("Tamamlanan proje sayısı negatif olamaz");
    }
}

public class UpdateAboutDtoValidator : AbstractValidator<UpdateAboutDto>
{
    public UpdateAboutDtoValidator()
    {
        RuleFor(x => x.Introduction)
            .NotEmpty().WithMessage("Tanıtım yazısı boş olamaz")
            .MaximumLength(2000).WithMessage("Tanıtım yazısı en fazla 2000 karakter olabilir");
    }
}

public class UpdateContactInfoDtoValidator : AbstractValidator<UpdateContactInfoDto>
{
    public UpdateContactInfoDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Geçerli bir e-posta adresi giriniz")
            .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir");
    }
}
