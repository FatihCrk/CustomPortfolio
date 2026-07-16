using FluentValidation;
using Portfolio.Shared.DTOs.CMS;

namespace Portfolio.Application.Validators;

public class CreateHeroSectionValidator : AbstractValidator<CreateHeroSectionDto>
{
    public CreateHeroSectionValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık gereklidir.")
            .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

        RuleFor(x => x.Subtitle)
            .MaximumLength(500).WithMessage("Alt başlık en fazla 500 karakter olabilir.");

        RuleFor(x => x.ButtonText)
            .MaximumLength(50).WithMessage("Buton metni en fazla 50 karakter olabilir.");

        RuleFor(x => x.ButtonUrl)
            .MaximumLength(500).WithMessage("Buton URL'si en fazla 500 karakter olabilir.");

        RuleFor(x => x.BackgroundImageUrl)
            .MaximumLength(500).WithMessage("Arkaplan görsel URL'si en fazla 500 karakter olabilir.");
    }
}

public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Proje başlığı gereklidir.")
            .MaximumLength(200).WithMessage("Proje başlığı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Proje açıklaması gereklidir.")
            .MaximumLength(5000).WithMessage("Açıklama en fazla 5000 karakter olabilir.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori gereklidir.")
            .MaximumLength(100).WithMessage("Kategori en fazla 100 karakter olabilir.");

        RuleFor(x => x.GithubUrl)
            .MaximumLength(500).WithMessage("GitHub URL'si en fazla 500 karakter olabilir.")
            .Matches(@"^https?://").When(x => !string.IsNullOrEmpty(x.GithubUrl)).WithMessage("Geçerli bir URL giriniz.");

        RuleFor(x => x.DemoUrl)
            .MaximumLength(500).WithMessage("Demo URL'si en fazla 500 karakter olabilir.")
            .Matches(@"^https?://").When(x => !string.IsNullOrEmpty(x.DemoUrl)).WithMessage("Geçerli bir URL giriniz.");
    }
}

public class CreateBlogPostValidator : AbstractValidator<CreateBlogPostDto>
{
    public CreateBlogPostValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Blog başlığı gereklidir.")
            .MaximumLength(300).WithMessage("Başlık en fazla 300 karakter olabilir.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug gereklidir.")
            .MaximumLength(400).WithMessage("Slug en fazla 400 karakter olabilir.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("İçerik gereklidir.")
            .MaximumLength(100000).WithMessage("İçerik çok uzun.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori gereklidir.")
            .MaximumLength(100).WithMessage("Kategori en fazla 100 karakter olabilir.");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Yazar ID gereklidir.");

        RuleFor(x => x.MetaTitle)
            .MaximumLength(70).WithMessage("Meta başlık en fazla 70 karakter olmalıdır (SEO).");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(160).WithMessage("Meta açıklama en fazla 160 karakter olmalıdır (SEO).");
    }
}

public class CreateContactMessageValidator : AbstractValidator<CreateContactMessageDto>
{
    public CreateContactMessageValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad soyad gereklidir.")
            .MinimumLength(2).WithMessage("Ad soyad en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi gereklidir.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Konu gereklidir.")
            .MaximumLength(200).WithMessage("Konu en fazla 200 karakter olabilir.");

        RuleFor(x => x.MessageBody)
            .NotEmpty().WithMessage("Mesaj gereklidir.")
            .MinimumLength(10).WithMessage("Mesaj en az 10 karakter olmalıdır.")
            .MaximumLength(5000).WithMessage("Mesaj en fazla 5000 karakter olabilir.");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.")
            .Matches(@"^\+?[\d\s\-\(\)]+$").When(x => !string.IsNullOrEmpty(x.Phone)).WithMessage("Geçerli bir telefon numarası giriniz.");
    }
}

public class CreateSkillValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Yetenek adı gereklidir.")
            .MaximumLength(100).WithMessage("Yetenek adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori gereklidir.")
            .MaximumLength(100).WithMessage("Kategori en fazla 100 karakter olabilir.");

        RuleFor(x => x.Level)
            .InclusiveBetween(1, 100).WithMessage("Seviye 1-100 arasında olmalıdır.");
    }
}

public class CreateExperienceValidator : AbstractValidator<CreateExperienceDto>
{
    public CreateExperienceValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Şirket adı gereklidir.")
            .MaximumLength(200).WithMessage("Şirket adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Pozisyon gereklidir.")
            .MaximumLength(200).WithMessage("Pozisyon en fazla 200 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue).WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.");
    }
}

public class CreateTestimonialValidator : AbstractValidator<CreateTestimonialDto>
{
    public CreateTestimonialValidator()
    {
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("Müşteri adı gereklidir.")
            .MaximumLength(100).WithMessage("Müşteri adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("Şirket adı gereklidir.")
            .MaximumLength(100).WithMessage("Şirket adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Yorum gereklidir.")
            .MaximumLength(1000).WithMessage("Yorum en fazla 1000 karakter olabilir.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Puanlama 1-5 arasında olmalıdır.");
    }
}

public class CreateSiteSettingsValidator : AbstractValidator<CreateSiteSettingsDto>
{
    public CreateSiteSettingsValidator()
    {
        RuleFor(x => x.SiteName)
            .NotEmpty().WithMessage("Site adı gereklidir.")
            .MaximumLength(100).WithMessage("Site adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.FooterText)
            .MaximumLength(500).WithMessage("Footer metni en fazla 500 karakter olabilir.");

        RuleFor(x => x.CopyrightText)
            .MaximumLength(200).WithMessage("Copyright metni en fazla 200 karakter olabilir.");

        RuleFor(x => x.PrimaryColor)
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage("Geçerli bir hex renk kodu giriniz (örn: #3b82f6).");

        RuleFor(x => x.SecondaryColor)
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage("Geçerli bir hex renk kodu giriniz (örn: #1e40af).");

        RuleFor(x => x.GoogleAnalyticsId)
            .MaximumLength(50).WithMessage("Google Analytics ID en fazla 50 karakter olabilir.")
            .Matches(@"^G-[A-Z0-9]+$").When(x => !string.IsNullOrEmpty(x.GoogleAnalyticsId)).WithMessage("Geçerli bir Google Analytics ID giriniz.");
    }
}
