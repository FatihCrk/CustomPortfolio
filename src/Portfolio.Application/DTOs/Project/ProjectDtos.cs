namespace Portfolio.Application.DTOs.Project;

/// <summary>
/// Proje DTO'si.
/// </summary>
public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public List<string> Technologies { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<ProjectImageDto> Images { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

/// <summary>
/// Proje oluşturma DTO'si.
/// </summary>
public class CreateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public List<string> Technologies { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}

/// <summary>
/// Proje güncelleme DTO'si.
/// </summary>
public class UpdateProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public List<string> Technologies { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}

/// <summary>
/// Proje görseli DTO'si.
/// </summary>
public class ProjectImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Sayfalı proje listesi response DTO'si.
/// </summary>
public class PagedProjectsResponseDto
{
    public List<ProjectDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}

/// <summary>
/// Proje filtreleme parametreleri.
/// </summary>
public class ProjectFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsFeatured { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; } = "DisplayOrder";
    public bool IsDescending { get; set; } = false;
}
