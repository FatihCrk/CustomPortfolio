using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Application.DTOs;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Entities;
using Portfolio.Persistence;

namespace Portfolio.Application.Services.Implementations;

public class SkillService : ISkillService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<SkillService> _logger;
    private readonly ICacheService _cacheService;

    public SkillService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<SkillService> logger,
        ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<SkillDto>> GetAllAsync(string? category = null)
    {
        var cacheKey = $"skills:{category ?? "all"}";
        
        var cached = await _cacheService.GetAsync<IEnumerable<SkillDto>>(cacheKey);
        if (cached != null)
            return cached;

        IQueryable<Skill> query = _context.Skills.Where(s => s.IsActive && !s.IsDeleted);

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(s => s.Category == category);
        }

        var skills = await query
            .OrderBy(s => s.Order)
            .ThenBy(s => s.Name)
            .ToListAsync();

        var dto = _mapper.Map<IEnumerable<SkillDto>>(skills);
        
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(30));
        
        return dto;
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        var cacheKey = "skills:categories";
        
        var cached = await _cacheService.GetAsync<IEnumerable<string>>(cacheKey);
        if (cached != null)
            return cached;

        var categories = await _context.Skills
            .Where(s => s.IsActive && !s.IsDeleted)
            .Select(s => s.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, categories, TimeSpan.FromHours(1));
        
        return categories;
    }

    public async Task<SkillDto?> GetByIdAsync(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        
        if (skill == null || skill.IsDeleted)
            return null;

        return _mapper.Map<SkillDto>(skill);
    }

    public async Task<SkillDto> CreateAsync(CreateSkillDto dto)
    {
        var skill = _mapper.Map<Skill>(dto);
        
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Skill created with ID: {Id}", skill.Id);
        
        // Clear cache
        await _cacheService.RemoveByPatternAsync("skills:*");

        return _mapper.Map<SkillDto>(skill);
    }

    public async Task<SkillDto> UpdateAsync(int id, UpdateSkillDto dto)
    {
        var skill = await _context.Skills.FindAsync(id);
        
        if (skill == null || skill.IsDeleted)
            throw new KeyNotFoundException($"Skill with ID {id} not found");

        _mapper.Map(dto, skill);
        
        await _context.SaveChangesAsync();

        _logger.LogInformation("Skill updated with ID: {Id}", id);
        
        // Clear cache
        await _cacheService.RemoveByPatternAsync("skills:*");

        return _mapper.Map<SkillDto>(skill);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        
        if (skill == null || skill.IsDeleted)
            return false;

        skill.IsDeleted = true;
        skill.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();

        _logger.LogInformation("Skill deleted with ID: {Id}", id);
        
        // Clear cache
        await _cacheService.RemoveByPatternAsync("skills:*");

        return true;
    }

    public async Task<bool> ReorderAsync(int id, int newOrder)
    {
        var skill = await _context.Skills.FindAsync(id);
        
        if (skill == null || skill.IsDeleted)
            return false;

        skill.Order = newOrder;
        
        await _context.SaveChangesAsync();

        // Clear cache
        await _cacheService.RemoveByPatternAsync("skills:*");

        return true;
    }
}
