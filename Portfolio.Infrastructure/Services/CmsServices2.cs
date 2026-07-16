using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Infrastructure.Services;

public class BlogService : IBlogService
{
    private readonly ApplicationDbContext _context;
    
    public BlogService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<BlogPostDto>> GetAllAsync(bool publishedOnly = false)
    {
        var query = _context.BlogPosts
            .Where(b => !b.IsDeleted)
            .AsQueryable();
            
        if (publishedOnly)
        {
            query = query.Where(b => b.IsPublished && b.PublishDate <= DateTime.UtcNow);
        }
        
        return await query
            .Include(b => b.Author)
            .Include(b => b.Category)
            .OrderByDescending(b => b.PublishDate)
            .Select(b => new BlogPostDto
            {
                Id = b.Id,
                Title = b.Title,
                Slug = b.Slug,
                Content = b.Content,
                Summary = b.Summary,
                CoverImageUrl = b.CoverImageUrl,
                IsPublished = b.IsPublished,
                PublishDate = b.PublishDate,
                ViewCount = b.ViewCount,
                MetaTitle = b.MetaTitle,
                MetaDescription = b.MetaDescription,
                MetaKeywords = b.MetaKeywords,
                AuthorId = b.AuthorId,
                AuthorName = b.Author != null ? b.Author.FullName : null,
                CategoryId = b.CategoryId,
                CategoryName = b.Category != null ? b.Category.Name : null,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            })
            .ToListAsync();
    }
    
    public async Task<PagedResult<BlogPostDto>> GetPagedAsync(int pageNumber, int pageSize, bool publishedOnly = false, int? categoryId = null, string? searchTerm = null)
    {
        var query = _context.BlogPosts
            .Where(b => !b.IsDeleted)
            .AsQueryable();
            
        if (publishedOnly)
        {
            query = query.Where(b => b.IsPublished && b.PublishDate <= DateTime.UtcNow);
        }
        
        if (categoryId.HasValue)
        {
            query = query.Where(b => b.CategoryId == categoryId.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(b => b.Title.Contains(searchTerm) || b.Content.Contains(searchTerm));
        }
        
        var totalRecords = await query.CountAsync();
        
        var items = await query
            .Include(b => b.Author)
            .Include(b => b.Category)
            .OrderByDescending(b => b.PublishDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BlogPostDto
            {
                Id = b.Id,
                Title = b.Title,
                Slug = b.Slug,
                Content = b.Content,
                Summary = b.Summary,
                CoverImageUrl = b.CoverImageUrl,
                IsPublished = b.IsPublished,
                PublishDate = b.PublishDate,
                ViewCount = b.ViewCount,
                MetaTitle = b.MetaTitle,
                MetaDescription = b.MetaDescription,
                MetaKeywords = b.MetaKeywords,
                AuthorId = b.AuthorId,
                AuthorName = b.Author != null ? b.Author.FullName : null,
                CategoryId = b.CategoryId,
                CategoryName = b.Category != null ? b.Category.Name : null,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            })
            .ToListAsync();
            
        return new PagedResult<BlogPostDto>
        {
            Items = items,
            TotalRecords = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }
    
    public async Task<BlogPostDto?> GetByIdAsync(int id)
    {
        var post = await _context.BlogPosts
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.Tags)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
            
        if (post == null) return null;
        
        return new BlogPostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Content = post.Content,
            Summary = post.Summary,
            CoverImageUrl = post.CoverImageUrl,
            IsPublished = post.IsPublished,
            PublishDate = post.PublishDate,
            ViewCount = post.ViewCount,
            MetaTitle = post.MetaTitle,
            MetaDescription = post.MetaDescription,
            MetaKeywords = post.MetaKeywords,
            AuthorId = post.AuthorId,
            AuthorName = post.Author != null ? post.Author.FullName : null,
            CategoryId = post.CategoryId,
            CategoryName = post.Category != null ? post.Category.Name : null,
            TagIds = post.Tags.Select(t => t.Id).ToList(),
            CreatedDate = post.CreatedDate,
            UpdatedDate = post.UpdatedDate
        };
    }
    
    public async Task<BlogPost?> GetBySlugAsync(string slug)
    {
        return await _context.BlogPosts
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.Tags)
            .FirstOrDefaultAsync(b => b.Slug == slug && !b.IsDeleted);
    }
    
    public async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        _context.BlogPosts.Add(blogPost);
        await _context.SaveChangesAsync();
        return blogPost;
    }
    
    public async Task<BlogPost?> UpdateAsync(int id, BlogPost blogPost)
    {
        var existing = await _context.BlogPosts.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Title = blogPost.Title;
        existing.Slug = blogPost.Slug;
        existing.Content = blogPost.Content;
        existing.Summary = blogPost.Summary;
        existing.CoverImageUrl = blogPost.CoverImageUrl;
        existing.IsPublished = blogPost.IsPublished;
        existing.PublishDate = blogPost.PublishDate;
        existing.MetaTitle = blogPost.MetaTitle;
        existing.MetaDescription = blogPost.MetaDescription;
        existing.MetaKeywords = blogPost.MetaKeywords;
        existing.CategoryId = blogPost.CategoryId;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post == null || post.IsDeleted) return false;
        
        post.IsDeleted = true;
        post.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task IncrementViewCountAsync(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post != null)
        {
            post.ViewCount++;
            await _context.SaveChangesAsync();
        }
    }
}

public class ContactInfoService : IContactInfoService
{
    private readonly ApplicationDbContext _context;
    
    public ContactInfoService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ContactInfoDto?> GetActiveAsync()
    {
        var info = await _context.ContactInfos
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.CreatedDate)
            .FirstOrDefaultAsync();
            
        if (info == null) return null;
        
        return new ContactInfoDto
        {
            Id = info.Id,
            Phone = info.Phone,
            Email = info.Email,
            Address = info.Address,
            City = info.City,
            Country = info.Country,
            PostalCode = info.PostalCode,
            MapLatitude = info.MapLatitude,
            MapLongitude = info.MapLongitude,
            CreatedDate = info.CreatedDate
        };
    }
    
    public async Task<ContactInfo> CreateAsync(ContactInfo contactInfo)
    {
        _context.ContactInfos.Add(contactInfo);
        await _context.SaveChangesAsync();
        return contactInfo;
    }
    
    public async Task<ContactInfo?> UpdateAsync(int id, ContactInfo contactInfo)
    {
        var existing = await _context.ContactInfos.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Phone = contactInfo.Phone;
        existing.Email = contactInfo.Email;
        existing.Address = contactInfo.Address;
        existing.City = contactInfo.City;
        existing.Country = contactInfo.Country;
        existing.PostalCode = contactInfo.PostalCode;
        existing.MapLatitude = contactInfo.MapLatitude;
        existing.MapLongitude = contactInfo.MapLongitude;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
}

public class SocialMediaService : ISocialMediaService
{
    private readonly ApplicationDbContext _context;
    
    public SocialMediaService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<SocialMediaDto>> GetAllAsync()
    {
        return await _context.SocialMedias
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Order)
            .Select(s => new SocialMediaDto
            {
                Id = s.Id,
                Platform = s.Platform,
                Url = s.Url,
                Icon = s.Icon,
                Order = s.Order,
                IsActive = s.IsActive,
                CreatedDate = s.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<SocialMedia?> GetByIdAsync(int id)
    {
        return await _context.SocialMedias
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }
    
    public async Task<SocialMedia> CreateAsync(SocialMedia socialMedia)
    {
        _context.SocialMedias.Add(socialMedia);
        await _context.SaveChangesAsync();
        return socialMedia;
    }
    
    public async Task<SocialMedia?> UpdateAsync(int id, SocialMedia socialMedia)
    {
        var existing = await _context.SocialMedias.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Platform = socialMedia.Platform;
        existing.Url = socialMedia.Url;
        existing.Icon = socialMedia.Icon;
        existing.Order = socialMedia.Order;
        existing.IsActive = socialMedia.IsActive;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var socialMedia = await _context.SocialMedias.FindAsync(id);
        if (socialMedia == null || socialMedia.IsDeleted) return false;
        
        socialMedia.IsDeleted = true;
        socialMedia.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}

public class HeroSectionService : IHeroSectionService
{
    private readonly ApplicationDbContext _context;
    
    public HeroSectionService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<HeroSectionDto?> GetActiveAsync()
    {
        var hero = await _context.HeroSections
            .Where(h => !h.IsDeleted && h.IsActive)
            .OrderByDescending(h => h.CreatedDate)
            .FirstOrDefaultAsync();
            
        if (hero == null) return null;
        
        return new HeroSectionDto
        {
            Id = hero.Id,
            Title = hero.Title,
            Subtitle = hero.Subtitle,
            Description = hero.Description,
            BackgroundImageUrl = hero.BackgroundImageUrl,
            ButtonText = hero.ButtonText,
            ButtonUrl = hero.ButtonUrl,
            ShowStats = hero.ShowStats,
            IsActive = hero.IsActive,
            Order = hero.Order,
            CreatedDate = hero.CreatedDate
        };
    }
    
    public async Task<HeroSection> CreateAsync(HeroSection heroSection)
    {
        _context.HeroSections.Add(heroSection);
        await _context.SaveChangesAsync();
        return heroSection;
    }
    
    public async Task<HeroSection?> UpdateAsync(int id, HeroSection heroSection)
    {
        var existing = await _context.HeroSections.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Title = heroSection.Title;
        existing.Subtitle = heroSection.Subtitle;
        existing.Description = heroSection.Description;
        existing.BackgroundImageUrl = heroSection.BackgroundImageUrl;
        existing.ButtonText = heroSection.ButtonText;
        existing.ButtonUrl = heroSection.ButtonUrl;
        existing.ShowStats = heroSection.ShowStats;
        existing.IsActive = heroSection.IsActive;
        existing.Order = heroSection.Order;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
}

public class AboutSectionService : IAboutSectionService
{
    private readonly ApplicationDbContext _context;
    
    public AboutSectionService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<AboutSectionDto?> GetActiveAsync()
    {
        var about = await _context.AboutSections
            .Where(a => !a.IsDeleted && a.IsActive)
            .OrderByDescending(a => a.CreatedDate)
            .FirstOrDefaultAsync();
            
        if (about == null) return null;
        
        return new AboutSectionDto
        {
            Id = about.Id,
            Title = about.Title,
            Subtitle = about.Subtitle,
            Description = about.Description,
            ShortBio = about.ShortBio,
            PhotoUrl = about.PhotoUrl,
            CvUrl = about.CvUrl,
            ExperienceYears = about.ExperienceYears,
            IsActive = about.IsActive,
            CreatedDate = about.CreatedDate
        };
    }
    
    public async Task<AboutSection> CreateAsync(AboutSection aboutSection)
    {
        _context.AboutSections.Add(aboutSection);
        await _context.SaveChangesAsync();
        return aboutSection;
    }
    
    public async Task<AboutSection?> UpdateAsync(int id, AboutSection aboutSection)
    {
        var existing = await _context.AboutSections.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Title = aboutSection.Title;
        existing.Subtitle = aboutSection.Subtitle;
        existing.Description = aboutSection.Description;
        existing.ShortBio = aboutSection.ShortBio;
        existing.PhotoUrl = aboutSection.PhotoUrl;
        existing.CvUrl = aboutSection.CvUrl;
        existing.ExperienceYears = aboutSection.ExperienceYears;
        existing.IsActive = aboutSection.IsActive;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
}

public class StatisticService : IStatisticService
{
    private readonly ApplicationDbContext _context;
    
    public StatisticService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<StatisticDto>> GetAllAsync()
    {
        return await _context.Statistics
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Order)
            .Select(s => new StatisticDto
            {
                Id = s.Id,
                Title = s.Title,
                Value = s.Value,
                Prefix = s.Prefix,
                Suffix = s.Suffix,
                Icon = s.Icon,
                Order = s.Order,
                CreatedDate = s.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<Statistic> CreateAsync(Statistic statistic)
    {
        _context.Statistics.Add(statistic);
        await _context.SaveChangesAsync();
        return statistic;
    }
    
    public async Task<Statistic?> UpdateAsync(int id, Statistic statistic)
    {
        var existing = await _context.Statistics.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Title = statistic.Title;
        existing.Value = statistic.Value;
        existing.Prefix = statistic.Prefix;
        existing.Suffix = statistic.Suffix;
        existing.Icon = statistic.Icon;
        existing.Order = statistic.Order;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var statistic = await _context.Statistics.FindAsync(id);
        if (statistic == null || statistic.IsDeleted) return false;
        
        statistic.IsDeleted = true;
        statistic.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}

public class SiteSettingService : ISiteSettingService
{
    private readonly ApplicationDbContext _context;
    
    public SiteSettingService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<SiteSettingDto?> GetActiveAsync()
    {
        var setting = await _context.SiteSettings
            .Where(s => !s.IsDeleted && s.IsActive)
            .OrderByDescending(s => s.CreatedDate)
            .FirstOrDefaultAsync();
            
        if (setting == null) return null;
        
        return new SiteSettingDto
        {
            Id = setting.Id,
            SiteName = setting.SiteName,
            SiteTitle = setting.SiteTitle,
            SiteDescription = setting.SiteDescription,
            LogoUrl = setting.LogoUrl,
            FaviconUrl = setting.FaviconUrl,
            FooterText = setting.FooterText,
            CopyrightText = setting.CopyrightText,
            PrimaryColor = setting.PrimaryColor,
            SecondaryColor = setting.SecondaryColor,
            FontFamily = setting.FontFamily,
            IsMaintenanceMode = setting.IsMaintenanceMode,
            MaintenanceMessage = setting.MaintenanceMessage,
            GoogleAnalyticsId = setting.GoogleAnalyticsId,
            FacebookPixelId = setting.FacebookPixelId,
            SmtpHost = setting.SmtpHost,
            SmtpPort = setting.SmtpPort,
            SmtpUser = setting.SmtpUser,
            SmtpPassword = setting.SmtpPassword,
            SmtpEnableSsl = setting.SmtpEnableSsl,
            FromEmail = setting.FromEmail,
            FromName = setting.FromName,
            IsActive = setting.IsActive,
            CreatedDate = setting.CreatedDate,
            UpdatedDate = setting.UpdatedDate
        };
    }
    
    public async Task<SiteSetting> CreateAsync(SiteSetting siteSetting)
    {
        _context.SiteSettings.Add(siteSetting);
        await _context.SaveChangesAsync();
        return siteSetting;
    }
    
    public async Task<SiteSetting?> UpdateAsync(int id, SiteSetting siteSetting)
    {
        var existing = await _context.SiteSettings.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.SiteName = siteSetting.SiteName;
        existing.SiteTitle = siteSetting.SiteTitle;
        existing.SiteDescription = siteSetting.SiteDescription;
        existing.LogoUrl = siteSetting.LogoUrl;
        existing.FaviconUrl = siteSetting.FaviconUrl;
        existing.FooterText = siteSetting.FooterText;
        existing.CopyrightText = siteSetting.CopyrightText;
        existing.PrimaryColor = siteSetting.PrimaryColor;
        existing.SecondaryColor = siteSetting.SecondaryColor;
        existing.FontFamily = siteSetting.FontFamily;
        existing.IsMaintenanceMode = siteSetting.IsMaintenanceMode;
        existing.MaintenanceMessage = siteSetting.MaintenanceMessage;
        existing.GoogleAnalyticsId = siteSetting.GoogleAnalyticsId;
        existing.FacebookPixelId = siteSetting.FacebookPixelId;
        existing.SmtpHost = siteSetting.SmtpHost;
        existing.SmtpPort = siteSetting.SmtpPort;
        existing.SmtpUser = siteSetting.SmtpUser;
        existing.SmtpPassword = siteSetting.SmtpPassword;
        existing.SmtpEnableSsl = siteSetting.SmtpEnableSsl;
        existing.FromEmail = siteSetting.FromEmail;
        existing.FromName = siteSetting.FromName;
        existing.IsActive = siteSetting.IsActive;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
}

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    
    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<CategoryDto>> GetAllAsync(string entityType = "Project")
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted && c.EntityType == entityType)
            .OrderBy(c => c.Order)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                Icon = c.Icon,
                EntityType = c.EntityType,
                Order = c.Order,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }
    
    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
    
    public async Task<Category?> UpdateAsync(int id, Category category)
    {
        var existing = await _context.Categories.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Name = category.Name;
        existing.Slug = category.Slug;
        existing.Description = category.Description;
        existing.Icon = category.Icon;
        existing.Order = category.Order;
        existing.IsActive = category.IsActive;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null || category.IsDeleted) return false;
        
        category.IsDeleted = true;
        category.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}
