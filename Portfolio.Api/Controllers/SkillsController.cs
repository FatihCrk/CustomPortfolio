using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;
using Portfolio.Application.Services.Interfaces;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : BaseApiController
{
    private readonly ISkillService _skillService;
    private readonly ILogger<SkillsController> _logger;

    public SkillsController(ISkillService skillService, ILogger<SkillsController> logger)
    {
        _skillService = skillService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<SkillDto>>>> GetAll([FromQuery] string? category)
    {
        try
        {
            var skills = await _skillService.GetAllAsync(category);
            return Ok(ApiResponse.Ok(skills, "Yetenekler getirildi"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skills");
            return StatusCode(500, ApiResponse<IEnumerable<SkillDto>>.Fail("Yetenekler getirilemedi"));
        }
    }

    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<string>>>> GetCategories()
    {
        try
        {
            var categories = await _skillService.GetCategoriesAsync();
            return Ok(ApiResponse.Ok(categories, "Kategoriler getirildi"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories");
            return StatusCode(500, ApiResponse<IEnumerable<string>>.Fail("Kategoriler getirilemedi"));
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<SkillDto>>> GetById(int id)
    {
        try
        {
            var skill = await _skillService.GetByIdAsync(id);
            
            if (skill == null)
                return NotFound(ApiResponse<SkillDto>.Fail("Yetenek bulunamadı"));

            return Ok(ApiResponse.Ok(skill, "Yetenek getirildi"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill by id");
            return StatusCode(500, ApiResponse<SkillDto>.Fail("Yetenek getirilemedi"));
        }
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<ActionResult<ApiResponse<SkillDto>>> Create([FromBody] CreateSkillDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SkillDto>.Fail("Geçersiz veri"));

            var skill = await _skillService.CreateAsync(dto);
            _logger.LogInformation("Skill created: {SkillName}", skill.Name);
            
            return CreatedAtAction(nameof(GetById), new { id = skill.Id }, ApiResponse.Ok(skill, "Yetenek oluşturuldu"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill");
            return StatusCode(500, ApiResponse<SkillDto>.Fail("Yetenek oluşturulamadı"));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<ActionResult<ApiResponse<SkillDto>>> Update(int id, [FromBody] UpdateSkillDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SkillDto>.Fail("Geçersiz veri"));

            var skill = await _skillService.UpdateAsync(id, dto);
            
            if (skill == null)
                return NotFound(ApiResponse<SkillDto>.Fail("Yetenek bulunamadı"));

            _logger.LogInformation("Skill updated: {SkillName}", skill.Name);
            return Ok(ApiResponse.Ok(skill, "Yetenek güncellendi"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating skill");
            return StatusCode(500, ApiResponse<SkillDto>.Fail("Yetenek güncellenemedi"));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var result = await _skillService.DeleteAsync(id);
            
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("Yetenek bulunamadı"));

            _logger.LogInformation("Skill deleted: {Id}", id);
            return Ok(ApiResponse.Ok(true, "Yetenek silindi"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting skill");
            return StatusCode(500, ApiResponse<bool>.Fail("Yetenek silinemedi"));
        }
    }

    [HttpPut("{id}/reorder")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<ActionResult<ApiResponse<bool>>> Reorder(int id, [FromBody] int newOrder)
    {
        try
        {
            var result = await _skillService.ReorderAsync(id, newOrder);
            
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("Yetenek bulunamadı"));

            return Ok(ApiResponse.Ok(true, "Sıralama güncellendi"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reordering skill");
            return StatusCode(500, ApiResponse<bool>.Fail("Sıralama güncellenemedi"));
        }
    }
}
