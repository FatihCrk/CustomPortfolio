using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Infrastructure.Services;

public class SkillService : ISkillService
{
    private readonly ApplicationDbContext _context;
    
    public SkillService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<SkillDto>> GetAllAsync(bool includeDetails = false)
    {
        var query = _context.Skills
            .Where(s => !s.IsDeleted)
            .AsQueryable();
            
        if (includeDetails)
        {
            query = query.Include(s => s.Category);
        }
        
        return await query
            .OrderBy(s => s.Order)
            .ThenByDescending(s => s.Percentage)
            .Select(s => new SkillDto
            {
                Id = s.Id,
                Name = s.Name,
                Percentage = s.Percentage,
                Icon = s.Icon,
                Order = s.Order,
                CategoryId = s.CategoryId,
                CategoryName = s.Category != null ? s.Category.Name : null,
                CreatedDate = s.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<Skill?> GetByIdAsync(int id)
    {
        return await _context.Skills
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }
    
    public async Task<Skill> CreateAsync(Skill skill)
    {
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();
        return skill;
    }
    
    public async Task<Skill?> UpdateAsync(int id, Skill skill)
    {
        var existingSkill = await _context.Skills.FindAsync(id);
        if (existingSkill == null || existingSkill.IsDeleted) return null;
        
        existingSkill.Name = skill.Name;
        existingSkill.Percentage = skill.Percentage;
        existingSkill.Icon = skill.Icon;
        existingSkill.Order = skill.Order;
        existingSkill.CategoryId = skill.CategoryId;
        existingSkill.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existingSkill;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null || skill.IsDeleted) return false;
        
        skill.IsDeleted = true;
        skill.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ExperienceService : IExperienceService
{
    private readonly ApplicationDbContext _context;
    
    public ExperienceService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ExperienceDto>> GetAllAsync()
    {
        return await _context.Experiences
            .Where(e => !e.IsDeleted)
            .OrderByDescending(e => e.StartDate)
            .Select(e => new ExperienceDto
            {
                Id = e.Id,
                Company = e.Company,
                LogoUrl = e.LogoUrl,
                Position = e.Position,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Description = e.Description,
                IsCurrent = e.IsCurrent,
                Order = e.Order,
                CreatedDate = e.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<Experience?> GetByIdAsync(int id)
    {
        return await _context.Experiences
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }
    
    public async Task<Experience> CreateAsync(Experience experience)
    {
        _context.Experiences.Add(experience);
        await _context.SaveChangesAsync();
        return experience;
    }
    
    public async Task<Experience?> UpdateAsync(int id, Experience experience)
    {
        var existing = await _context.Experiences.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Company = experience.Company;
        existing.LogoUrl = experience.LogoUrl;
        existing.Position = experience.Position;
        existing.StartDate = experience.StartDate;
        existing.EndDate = experience.EndDate;
        existing.Description = experience.Description;
        existing.IsCurrent = experience.IsCurrent;
        existing.Order = experience.Order;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var experience = await _context.Experiences.FindAsync(id);
        if (experience == null || experience.IsDeleted) return false;
        
        experience.IsDeleted = true;
        experience.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}

public class EducationService : IEducationService
{
    private readonly ApplicationDbContext _context;
    
    public EducationService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<EducationDto>> GetAllAsync()
    {
        return await _context.Educations
            .Where(e => !e.IsDeleted)
            .OrderByDescending(e => e.StartDate)
            .Select(e => new EducationDto
            {
                Id = e.Id,
                School = e.School,
                Department = e.Department,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                DiplomaUrl = e.DiplomaUrl,
                Description = e.Description,
                IsCurrent = e.IsCurrent,
                Order = e.Order,
                CreatedDate = e.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<Education?> GetByIdAsync(int id)
    {
        return await _context.Educations
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }
    
    public async Task<Education> CreateAsync(Education education)
    {
        _context.Educations.Add(education);
        await _context.SaveChangesAsync();
        return education;
    }
    
    public async Task<Education?> UpdateAsync(int id, Education education)
    {
        var existing = await _context.Educations.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.School = education.School;
        existing.Department = education.Department;
        existing.StartDate = education.StartDate;
        existing.EndDate = education.EndDate;
        existing.DiplomaUrl = education.DiplomaUrl;
        existing.Description = education.Description;
        existing.IsCurrent = education.IsCurrent;
        existing.Order = education.Order;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var education = await _context.Educations.FindAsync(id);
        if (education == null || education.IsDeleted) return false;
        
        education.IsDeleted = true;
        education.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}

public class CertificateService : ICertificateService
{
    private readonly ApplicationDbContext _context;
    
    public CertificateService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<CertificateDto>> GetAllAsync()
    {
        return await _context.Certificates
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.IssueDate)
            .Select(c => new CertificateDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                FileUrl = c.FileUrl,
                Issuer = c.Issuer,
                IssueDate = c.IssueDate,
                ExpirationDate = c.ExpirationDate,
                CredentialId = c.CredentialId,
                CredentialUrl = c.CredentialUrl,
                Order = c.Order,
                CreatedDate = c.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<Certificate?> GetByIdAsync(int id)
    {
        return await _context.Certificates
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }
    
    public async Task<Certificate> CreateAsync(Certificate certificate)
    {
        _context.Certificates.Add(certificate);
        await _context.SaveChangesAsync();
        return certificate;
    }
    
    public async Task<Certificate?> UpdateAsync(int id, Certificate certificate)
    {
        var existing = await _context.Certificates.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Title = certificate.Title;
        existing.Description = certificate.Description;
        existing.ImageUrl = certificate.ImageUrl;
        existing.FileUrl = certificate.FileUrl;
        existing.Issuer = certificate.Issuer;
        existing.IssueDate = certificate.IssueDate;
        existing.ExpirationDate = certificate.ExpirationDate;
        existing.CredentialId = certificate.CredentialId;
        existing.CredentialUrl = certificate.CredentialUrl;
        existing.Order = certificate.Order;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var certificate = await _context.Certificates.FindAsync(id);
        if (certificate == null || certificate.IsDeleted) return false;
        
        certificate.IsDeleted = true;
        certificate.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ReferenceService : IReferenceService
{
    private readonly ApplicationDbContext _context;
    
    public ReferenceService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ReferenceDto>> GetAllAsync()
    {
        return await _context.References
            .Where(r => !r.IsDeleted)
            .OrderBy(r => r.Order)
            .Select(r => new ReferenceDto
            {
                Id = r.Id,
                Name = r.Name,
                Title = r.Title,
                Company = r.Company,
                PhotoUrl = r.PhotoUrl,
                Comment = r.Comment,
                Order = r.Order,
                CreatedDate = r.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<Reference?> GetByIdAsync(int id)
    {
        return await _context.References
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }
    
    public async Task<Reference> CreateAsync(Reference reference)
    {
        _context.References.Add(reference);
        await _context.SaveChangesAsync();
        return reference;
    }
    
    public async Task<Reference?> UpdateAsync(int id, Reference reference)
    {
        var existing = await _context.References.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Name = reference.Name;
        existing.Title = reference.Title;
        existing.Company = reference.Company;
        existing.PhotoUrl = reference.PhotoUrl;
        existing.Comment = reference.Comment;
        existing.Order = reference.Order;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var reference = await _context.References.FindAsync(id);
        if (reference == null || reference.IsDeleted) return false;
        
        reference.IsDeleted = true;
        reference.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ServiceItemService : IServiceItemService
{
    private readonly ApplicationDbContext _context;
    
    public ServiceItemService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ServiceItemDto>> GetAllAsync()
    {
        return await _context.ServiceItems
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Order)
            .Select(s => new ServiceItemDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Icon = s.Icon,
                Order = s.Order,
                CreatedDate = s.CreatedDate
            })
            .ToListAsync();
    }
    
    public async Task<ServiceItem?> GetByIdAsync(int id)
    {
        return await _context.ServiceItems
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }
    
    public async Task<ServiceItem> CreateAsync(ServiceItem serviceItem)
    {
        _context.ServiceItems.Add(serviceItem);
        await _context.SaveChangesAsync();
        return serviceItem;
    }
    
    public async Task<ServiceItem?> UpdateAsync(int id, ServiceItem serviceItem)
    {
        var existing = await _context.ServiceItems.FindAsync(id);
        if (existing == null || existing.IsDeleted) return null;
        
        existing.Title = serviceItem.Title;
        existing.Description = serviceItem.Description;
        existing.Icon = serviceItem.Icon;
        existing.Order = serviceItem.Order;
        existing.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var serviceItem = await _context.ServiceItems.FindAsync(id);
        if (serviceItem == null || serviceItem.IsDeleted) return false;
        
        serviceItem.IsDeleted = true;
        serviceItem.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}
