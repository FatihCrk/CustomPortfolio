using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Portfolio.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;
    
    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ProjectDto>> GetAllAsync(bool includeDetails = false)
    {
        var query = _context.Projects
            .Where(p => !p.IsDeleted)
            .AsQueryable();
            
        if (includeDetails)
        {
            query = query
                .Include(p => p.Category)
                .Include(p => p.Technologies);
        }
        
        return await query
            .OrderByDescending(p => p.CreatedDate)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                GithubUrl = p.GithubUrl,
                DemoUrl = p.DemoUrl,
                IsFeatured = p.IsFeatured,
                Order = p.Order,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            })
            .ToListAsync();
    }
    
    public async Task<PagedResult<ProjectDto>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, int? categoryId = null)
    {
        var query = _context.Projects
            .Where(p => !p.IsDeleted)
            .AsQueryable();
            
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Title.Contains(searchTerm) || p.Description.Contains(searchTerm));
        }
        
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }
        
        var totalRecords = await query.CountAsync();
        
        var items = await query
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                GithubUrl = p.GithubUrl,
                DemoUrl = p.DemoUrl,
                IsFeatured = p.IsFeatured,
                Order = p.Order,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            })
            .ToListAsync();
            
        return new PagedResult<ProjectDto>
        {
            Items = items,
            TotalRecords = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }
    
    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _context.Projects
            .Where(p => p.Id == id && !p.IsDeleted)
            .Include(p => p.Category)
            .Include(p => p.Technologies)
            .FirstOrDefaultAsync();
            
        if (project == null) return null;
        
        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            ImageUrl = project.ImageUrl,
            GithubUrl = project.GithubUrl,
            DemoUrl = project.DemoUrl,
            IsFeatured = project.IsFeatured,
            Order = project.Order,
            CategoryId = project.CategoryId,
            CategoryName = project.Category != null ? project.Category.Name : null,
            TechnologyIds = project.Technologies.Select(t => t.Id).ToList(),
            CreatedDate = project.CreatedDate,
            UpdatedDate = project.UpdatedDate
        };
    }
    
    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }
    
    public async Task<Project?> UpdateAsync(int id, Project project)
    {
        var existingProject = await _context.Projects.FindAsync(id);
        if (existingProject == null || existingProject.IsDeleted) return null;
        
        existingProject.Title = project.Title;
        existingProject.Description = project.Description;
        existingProject.ImageUrl = project.ImageUrl;
        existingProject.GithubUrl = project.GithubUrl;
        existingProject.DemoUrl = project.DemoUrl;
        existingProject.IsFeatured = project.IsFeatured;
        existingProject.Order = project.Order;
        existingProject.CategoryId = project.CategoryId;
        existingProject.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return existingProject;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null || project.IsDeleted) return false;
        
        project.IsDeleted = true;
        project.DeletedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;
    
    public MessageService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Message> CreateAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }
    
    public async Task<PagedResult<MessageDto>> GetPagedAsync(int pageNumber, int pageSize, MessageStatus? status = null)
    {
        var query = _context.Messages
            .AsQueryable();
            
        if (status.HasValue)
        {
            query = query.Where(m => m.Status == status.Value);
        }
        
        var totalRecords = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(m => m.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Subject = m.Subject,
                MessageContent = m.MessageContent,
                Status = m.Status,
                IpAddress = m.IpAddress,
                UserAgent = m.UserAgent,
                CreatedDate = m.CreatedDate,
                ReadDate = m.ReadDate,
                RepliedDate = m.RepliedDate
            })
            .ToListAsync();
            
        return new PagedResult<MessageDto>
        {
            Items = items,
            TotalRecords = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }
    
    public async Task<MessageDto?> GetByIdAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return null;
        
        return new MessageDto
        {
            Id = message.Id,
            Name = message.Name,
            Email = message.Email,
            Phone = message.Phone,
            Subject = message.Subject,
            MessageContent = message.MessageContent,
            Status = message.Status,
            IpAddress = message.IpAddress,
            UserAgent = message.UserAgent,
            CreatedDate = message.CreatedDate,
            ReadDate = message.ReadDate,
            RepliedDate = message.RepliedDate
        };
    }
    
    public async Task<bool> MarkAsReadAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return false;
        
        message.Status = MessageStatus.Read;
        message.ReadDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> MarkAsRepliedAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return false;
        
        message.Status = MessageStatus.Replied;
        message.RepliedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> ArchiveAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return false;
        
        message.Status = MessageStatus.Archived;
        
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return false;
        
        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<int> GetUnreadCountAsync()
    {
        return await _context.Messages.CountAsync(m => m.Status == MessageStatus.Unread);
    }
}
