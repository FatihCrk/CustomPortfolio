using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entities.Cms;
using Portfolio.Api.Controllers;
using Portfolio.Shared.Response;

namespace Portfolio.Api.Controllers;

public class ProjectsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Tüm projeleri listeler (public)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProjects(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? category = null,
        CancellationToken cancellationToken = default)
    {
        var projects = await _unitOfWork.Project.GetAllAsync(
            p => !p.IsDeleted && 
                 (string.IsNullOrEmpty(search) || p.Title.Contains(search)) &&
                 (string.IsNullOrEmpty(category) || p.Category == category),
            orderBy: q => q.OrderByDescending(x => x.CreatedDate),
            pageNumber: page,
            pageSize: pageSize,
            cancellationToken: cancellationToken);

        var totalCount = await _unitOfWork.Project.CountAsync(
            p => !p.IsDeleted &&
                 (string.IsNullOrEmpty(search) || p.Title.Contains(search)) &&
                 (string.IsNullOrEmpty(category) || p.Category == category),
            cancellationToken);

        var response = new PagedResponse<Project>
        {
            Success = true,
            Data = projects.ToList(),
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            Message = "Projeler başarıyla getirildi"
        };

        return Ok(response);
    }

    /// <summary>
    /// Proje detayını getirir (public)
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProject(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _unitOfWork.Project.GetByIdAsync(id, cancellationToken);
        
        if (project == null || project.IsDeleted)
            return NotFound("Proje bulunamadı");

        return Ok(new ApiResponse<Project>
        {
            Success = true,
            Data = project,
            Message = "Proje detayı getirildi"
        });
    }

    /// <summary>
    /// Yeni proje oluşturur (admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto dto, CancellationToken cancellationToken = default)
    {
        // TODO: FluentValidation ile doğrulama
        // TODO: Slug oluşturma
        
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            GithubUrl = dto.GithubUrl,
            DemoUrl = dto.DemoUrl,
            ImageUrl = dto.ImageUrl,
            Technologies = dto.Technologies,
            Tags = dto.Tags,
            IsFeatured = dto.IsFeatured,
            SortOrder = dto.SortOrder,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Project.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Created(nameof(GetProject), new { id = project.Id }, "Proje başarıyla oluşturuldu");
    }

    /// <summary>
    /// Projeyi günceller (admin only)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var project = await _unitOfWork.Project.GetByIdAsync(id, cancellationToken);
        
        if (project == null || project.IsDeleted)
            return NotFound("Proje bulunamadı");

        // Güncelleme
        project.Title = dto.Title ?? project.Title;
        project.Description = dto.Description ?? project.Description;
        project.Category = dto.Category ?? project.Category;
        project.GithubUrl = dto.GithubUrl ?? project.GithubUrl;
        project.DemoUrl = dto.DemoUrl ?? project.DemoUrl;
        project.ImageUrl = dto.ImageUrl ?? project.ImageUrl;
        project.Technologies = dto.Technologies ?? project.Technologies;
        project.Tags = dto.Tags ?? project.Tags;
        project.IsFeatured = dto.IsFeatured ?? project.IsFeatured;
        project.SortOrder = dto.SortOrder ?? project.SortOrder;
        project.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.Project.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(new ApiResponse<Project>
        {
            Success = true,
            Data = project,
            Message = "Proje başarıyla güncellendi"
        });
    }

    /// <summary>
    /// Projeyi siler (soft delete) (admin only)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> DeleteProject(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _unitOfWork.Project.GetByIdAsync(id, cancellationToken);
        
        if (project == null || project.IsDeleted)
            return NotFound("Proje bulunamadı");

        _unitOfWork.Project.SoftDelete(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Proje başarıyla silindi"
        });
    }
}

// DTOs
public class ProjectCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string? ImageUrl { get; set; }
    public List<string>? Technologies { get; set; }
    public List<string>? Tags { get; set; }
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
}

public class ProjectUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string? ImageUrl { get; set; }
    public List<string>? Technologies { get; set; }
    public List<string>? Tags { get; set; }
    public bool? IsFeatured { get; set; }
    public int? SortOrder { get; set; }
}
