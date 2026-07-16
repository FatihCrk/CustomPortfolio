using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Persistence;
using System.Text;
using System.Text.RegularExpressions;

namespace Portfolio.Infrastructure.Services;

public class SlugService : ISlugService
{
    private readonly ApplicationDbContext _context;

    public SlugService(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Generate(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Convert to lowercase
        var slug = text.ToLowerInvariant();

        // Remove Turkish characters
        slug = ReplaceTurkishCharacters(slug);

        // Remove special characters
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", string.Empty);

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");

        // Replace multiple hyphens with single hyphen
        slug = Regex.Replace(slug, @"-+", "-");

        // Trim hyphens from start and end
        slug = slug.Trim('-');

        // Ensure minimum length
        if (slug.Length < 3)
        {
            slug += $"-{Guid.NewGuid().ToString("N").Substring(0, 6)}";
        }

        return slug;
    }

    public async Task<string> GenerateUniqueAsync(string text, CancellationToken cancellationToken = default)
    {
        var baseSlug = Generate(text);
        var slug = baseSlug;
        var counter = 1;

        // Check uniqueness based on entity type
        // This is a generic implementation - in production, you might want to check specific tables
        while (await IsSlugExistsAsync(slug, cancellationToken))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    private string ReplaceTurkishCharacters(string text)
    {
        var turkishChars = new Dictionary<string, string>
        {
            { "ç", "c" },
            { "ğ", "g" },
            { "ı", "i" },
            { "ö", "o" },
            { "ş", "s" },
            { "ü", "u" },
            { "Ç", "C" },
            { "Ğ", "G" },
            { "İ", "I" },
            { "Ö", "O" },
            { "Ş", "S" },
            { "Ü", "U" }
        };

        foreach (var pair in turkishChars)
        {
            text = text.Replace(pair.Key, pair.Value);
        }

        return text;
    }

    private async Task<bool> IsSlugExistsAsync(string slug, CancellationToken cancellationToken)
    {
        // Check in Posts table (most common use case for slugs)
        // In production, you might want to make this more generic or check multiple tables
        
        try
        {
            // Check if slug exists in Posts
            var postExists = await _context.Posts
                .AnyAsync(p => p.Slug == slug, cancellationToken);

            if (postExists)
                return true;

            // Add more checks here for other entities that use slugs
            // e.g., Categories, Tags, etc.

            return false;
        }
        catch
        {
            // If there's an error checking, assume it doesn't exist
            // This prevents blocking slug generation on DB errors
            return false;
        }
    }
}
