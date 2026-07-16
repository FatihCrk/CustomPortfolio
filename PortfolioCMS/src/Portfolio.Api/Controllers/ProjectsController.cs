using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Enums;

namespace Portfolio.Api.Controllers;

public class ProjectsController : BaseApiController
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProjectDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ProjectDto>>>> GetProjects(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] bool? isFeatured = null,
        [FromQuery] string sortBy = "CreatedDate",
        [FromQuery] bool sortDescending = true)
    {
        var result = await _projectService.GetProjectsAsync(
            pageNumber, pageSize, searchTerm, categoryId, isFeatured, sortBy, sortDescending);

        return Success(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProject(int id)
    {
        var result = await _projectService.GetProjectByIdAsync(id);

        if (result == null)
        {
            return NotFound(Error("Proje bulunamadı"));
        }

        return Success(result);
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProjectBySlug(string slug)
    {
        var result = await _projectService.GetProjectBySlugAsync(slug);

        if (result == null)
        {
            return NotFound(Error("Proje bulunamadı"));
        }

        return Success(result);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> CreateProject([FromBody] CreateProjectDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _projectService.CreateProjectAsync(dto, userId);

        _logger.LogInformation("Project created: {Title} by User {UserId}", dto.Title, userId);
        return CreatedAtAction(nameof(GetProject), new { id = result.Id }, Success(result, "Proje oluşturuldu"));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> UpdateProject(int id, [FromBody] UpdateProjectDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(Error("ID eşleşmiyor"));
        }

        var userId = GetCurrentUserId();
        var result = await _projectService.UpdateProjectAsync(dto, userId);

        if (result == null)
        {
            return NotFound(Error("Proje bulunamadı"));
        }

        _logger.LogInformation("Project updated: {Title} by User {UserId}", dto.Title, userId);
        return Success(result, "Proje güncellendi");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> DeleteProject(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _projectService.DeleteProjectAsync(id, userId);

        if (!result)
        {
            return NotFound(Error("Proje bulunamadı"));
        }

        _logger.LogInformation("Project deleted: ID {Id} by User {UserId}", id, userId);
        return Success("Proje silindi");
    }

    [HttpPut("{id}/toggle-featured")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> ToggleFeatured(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _projectService.ToggleFeaturedAsync(id, userId);

        if (result == null)
        {
            return NotFound(Error("Proje bulunamadı"));
        }

        return Success(result, "Öne çıkarılma durumu değiştirildi");
    }
}
