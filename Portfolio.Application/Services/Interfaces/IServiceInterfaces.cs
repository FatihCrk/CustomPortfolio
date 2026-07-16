using Portfolio.Domain.Entities;
using System.Linq.Expressions;

namespace Portfolio.Application.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync(bool includeDetails = false);
    Task<IEnumerable<ProjectDto>> GetFeaturedAsync(int count = 6);
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<ProjectDto> CreateAsync(CreateProjectDto dto);
    Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ToggleFeaturedAsync(int id);
}

public interface IMessageService
{
    Task<IEnumerable<MessageDto>> GetAllAsync(string? status = null, string? search = null);
    Task<MessageDto?> GetByIdAsync(int id);
    Task<MessageDto> CreateAsync(CreateMessageDto dto, string ipAddress, string userAgent);
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> MarkAsUnreadAsync(int id);
    Task<bool> ArchiveAsync(int id);
    Task<bool> UnarchiveAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<bool> ReplyAsync(int id, string reply);
    Task<byte[]> ExportToCsvAsync(IEnumerable<int> ids);
}

public interface ISkillService
{
    Task<IEnumerable<SkillDto>> GetAllAsync(string? category = null);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<SkillDto?> GetByIdAsync(int id);
    Task<SkillDto> CreateAsync(CreateSkillDto dto);
    Task<SkillDto> UpdateAsync(int id, UpdateSkillDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ReorderAsync(int id, int newOrder);
}

public interface IExperienceService
{
    Task<IEnumerable<ExperienceDto>> GetAllAsync();
    Task<ExperienceDto?> GetByIdAsync(int id);
    Task<ExperienceDto> CreateAsync(CreateExperienceDto dto);
    Task<ExperienceDto> UpdateAsync(int id, UpdateExperienceDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IEducationService
{
    Task<IEnumerable<EducationDto>> GetAllAsync();
    Task<EducationDto?> GetByIdAsync(int id);
    Task<EducationDto> CreateAsync(CreateEducationDto dto);
    Task<EducationDto> UpdateAsync(int id, UpdateEducationDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IBlogService
{
    Task<IEnumerable<PostDto>> GetAllAsync(bool publishedOnly = true, string? category = null, string? search = null);
    Task<PostDto?> GetBySlugAsync(string slug);
    Task<PostDto?> GetByIdAsync(int id);
    Task<PostDto> CreateAsync(CreatePostDto dto, string authorId);
    Task<PostDto> UpdateAsync(int id, UpdatePostDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> PublishAsync(int id);
    Task<bool> UnpublishAsync(int id);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<IEnumerable<string>> GetTagsAsync();
}

public interface IReferenceService
{
    Task<IEnumerable<ReferenceDto>> GetAllAsync();
    Task<ReferenceDto?> GetByIdAsync(int id);
    Task<ReferenceDto> CreateAsync(CreateReferenceDto dto);
    Task<ReferenceDto> UpdateAsync(int id, UpdateReferenceDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface ICertificateService
{
    Task<IEnumerable<CertificateDto>> GetAllAsync();
    Task<CertificateDto?> GetByIdAsync(int id);
    Task<CertificateDto> CreateAsync(CreateCertificateDto dto);
    Task<CertificateDto> UpdateAsync(int id, UpdateCertificateDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IServiceItemService
{
    Task<IEnumerable<ServiceItemDto>> GetAllAsync();
    Task<ServiceItemDto?> GetByIdAsync(int id);
    Task<ServiceItemDto> CreateAsync(CreateServiceItemDto dto);
    Task<ServiceItemDto> UpdateAsync(int id, UpdateServiceItemDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IContactInfoService
{
    Task<ContactInfoDto> GetAsync();
    Task<ContactInfoDto> UpdateAsync(UpdateContactInfoDto dto);
}

public interface ISocialMediaService
{
    Task<IEnumerable<SocialMediaDto>> GetAllAsync();
    Task<SocialMediaDto?> GetByIdAsync(int id);
    Task<SocialMediaDto> CreateAsync(CreateSocialMediaDto dto);
    Task<SocialMediaDto> UpdateAsync(int id, UpdateSocialMediaDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<SocialMediaDto>> GetActiveAsync();
}

public interface IHeroService
{
    Task<HeroDto> GetAsync();
    Task<HeroDto> UpdateAsync(UpdateHeroDto dto);
}

public interface IAboutService
{
    Task<AboutDto> GetAsync();
    Task<AboutDto> UpdateAsync(UpdateAboutDto dto);
}

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder = "uploads");
    Task<bool> DeleteAsync(string filePath);
    Task<byte[]> DownloadAsync(string filePath);
    Task<bool> ExistsAsync(string filePath);
    Task<string> GetUrlAsync(string filePath);
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task RemoveByPatternAsync(string pattern);
}

public interface ISlugService
{
    string Generate(string text);
    Task<string> GenerateUniqueAsync(string text, CancellationToken cancellationToken = default);
}

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task SendPasswordResetAsync(string to, string email, string resetToken);
    Task SendContactNotificationAsync(string adminEmail, MessageDto message);
}

public interface ISetupService
{
    Task<bool> IsSetupCompletedAsync();
    Task<SetupResultDto> CompleteSetupAsync(SetupAdminDto dto);
}

public interface IAuditLogService
{
    Task<IEnumerable<AuditLogDto>> GetAllAsync(string? entityType = null, string? action = null, string? userId = null);
    Task<AuditLogDto?> GetByIdAsync(long id);
}
