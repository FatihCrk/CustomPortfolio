using System.Text;
using System.Text.RegularExpressions;
using Portfolio.Application.Interfaces;
using Portfolio.Persistence;

namespace Portfolio.Infrastructure.Services;

public partial class SlugService : ISlugService
{
    private readonly ApplicationDbContext _context;

    public SlugService(ApplicationDbContext context)
    {
        _context = context;
    }

    public string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Türkçe karakterleri dönüştür
        text = NormalizeText(text);

        // Küçük harfe çevir
        text = text.ToLowerInvariant();

        // Geçersiz karakterleri kaldır
        text = InvalidCharsRegex().Replace(text, "");

        // Boşlukları tire ile değiştir
        text = Regex.Replace(text, @"\s+", "-");

        // Birden fazla tireyi tek tireye indirge
        text = Regex.Replace(text, @"-+", "-");

        // Baş ve sondaki tireleri kaldır
        text = text.Trim('-');

        return text;
    }

    public async Task<string> GenerateUniqueSlugAsync<T>(string text, CancellationToken cancellationToken = default) where T : class
    {
        var slug = GenerateSlug(text);
        var originalSlug = slug;
        var counter = 1;

        // Entity türüne göre tablonu adını bul
        var entityName = typeof(T).Name;

        while (await SlugExistsAsync<T>(slug, cancellationToken))
        {
            slug = $"{originalSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    private async Task<bool> SlugExistsAsync<T>(string slug, CancellationToken cancellationToken) where T : class
    {
        // Slug kontrolü için entity türüne göre dinamik sorgu
        // Basitleştirilmiş versiyon - gerçek implementasyonda reflection veya expression tree kullanılabilir
        return await Task.FromResult(false);
    }

    private static string NormalizeText(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                switch (c)
                {
                    case 'İ':
                        stringBuilder.Append("i");
                        break;
                    case 'ı':
                        stringBuilder.Append("i");
                        break;
                    case 'Ş':
                        stringBuilder.Append("s");
                        break;
                    case 'ş':
                        stringBuilder.Append("s");
                        break;
                    case 'Ç':
                        stringBuilder.Append("c");
                        break;
                    case 'ç':
                        stringBuilder.Append("c");
                        break;
                    case 'Ğ':
                        stringBuilder.Append("g");
                        break;
                    case 'ğ':
                        stringBuilder.Append("g");
                        break;
                    case 'Ö':
                        stringBuilder.Append("o");
                        break;
                    case 'ö':
                        stringBuilder.Append("o");
                        break;
                    case 'Ü':
                        stringBuilder.Append("u");
                        break;
                    case 'ü':
                        stringBuilder.Append("u");
                        break;
                    default:
                        stringBuilder.Append(c);
                        break;
                }
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex InvalidCharsRegex();
}
