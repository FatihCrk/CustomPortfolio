using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entities;
using System.Security.Claims;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class SkillsController : ControllerBase
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
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var skills = await _skillService.GetAllAsync(includeDetails: true);
            return Ok(new ApiResponse<IEnumerable<SkillDto>>(true, "Skills retrieved successfully", skills));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving skills");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving skills"));
        }
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var skill = await _skillService.GetByIdAsync(id);
            if (skill == null)
                return NotFound(new ApiResponse<object>(false, "Skill not found"));
                
            return Ok(new ApiResponse<SkillDto>(true, "Skill retrieved successfully", skill));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving skill {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving skill"));
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Create([FromBody] Skill skill)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var createdSkill = await _skillService.CreateAsync(skill);
            return CreatedAtAction(nameof(GetById), new { id = createdSkill.Id }, 
                new ApiResponse<Skill>(true, "Skill created successfully", createdSkill));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while creating skill"));
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Update(int id, [FromBody] Skill skill)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var updatedSkill = await _skillService.UpdateAsync(id, skill);
            if (updatedSkill == null)
                return NotFound(new ApiResponse<object>(false, "Skill not found or already deleted"));
                
            return Ok(new ApiResponse<Skill>(true, "Skill updated successfully", updatedSkill));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating skill {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while updating skill"));
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _skillService.DeleteAsync(id);
            if (!result)
                return NotFound(new ApiResponse<object>(false, "Skill not found or already deleted"));
                
            return Ok(new ApiResponse<object>(true, "Skill deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting skill {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while deleting skill"));
        }
    }
}

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ExperiencesController : ControllerBase
{
    private readonly IExperienceService _experienceService;
    private readonly ILogger<ExperiencesController> _logger;
    
    public ExperiencesController(IExperienceService experienceService, ILogger<ExperiencesController> logger)
    {
        _experienceService = experienceService;
        _logger = logger;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var experiences = await _experienceService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ExperienceDto>>(true, "Experiences retrieved successfully", experiences));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving experiences");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving experiences"));
        }
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var experience = await _experienceService.GetByIdAsync(id);
            if (experience == null)
                return NotFound(new ApiResponse<object>(false, "Experience not found"));
                
            return Ok(new ApiResponse<ExperienceDto>(true, "Experience retrieved successfully", experience));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving experience {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving experience"));
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Create([FromBody] Experience experience)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var createdExperience = await _experienceService.CreateAsync(experience);
            return CreatedAtAction(nameof(GetById), new { id = createdExperience.Id }, 
                new ApiResponse<Experience>(true, "Experience created successfully", createdExperience));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating experience");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while creating experience"));
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Update(int id, [FromBody] Experience experience)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var updatedExperience = await _experienceService.UpdateAsync(id, experience);
            if (updatedExperience == null)
                return NotFound(new ApiResponse<object>(false, "Experience not found or already deleted"));
                
            return Ok(new ApiResponse<Experience>(true, "Experience updated successfully", updatedExperience));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating experience {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while updating experience"));
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _experienceService.DeleteAsync(id);
            if (!result)
                return NotFound(new ApiResponse<object>(false, "Experience not found or already deleted"));
                
            return Ok(new ApiResponse<object>(true, "Experience deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting experience {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while deleting experience"));
        }
    }
}

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EducationsController : ControllerBase
{
    private readonly IEducationService _educationService;
    private readonly ILogger<EducationsController> _logger;
    
    public EducationsController(IEducationService educationService, ILogger<EducationsController> logger)
    {
        _educationService = educationService;
        _logger = logger;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var educations = await _educationService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<EducationDto>>(true, "Educations retrieved successfully", educations));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving educations");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving educations"));
        }
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var education = await _educationService.GetByIdAsync(id);
            if (education == null)
                return NotFound(new ApiResponse<object>(false, "Education not found"));
                
            return Ok(new ApiResponse<EducationDto>(true, "Education retrieved successfully", education));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving education {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving education"));
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Create([FromBody] Education education)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var createdEducation = await _educationService.CreateAsync(education);
            return CreatedAtAction(nameof(GetById), new { id = createdEducation.Id }, 
                new ApiResponse<Education>(true, "Education created successfully", createdEducation));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating education");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while creating education"));
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Update(int id, [FromBody] Education education)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var updatedEducation = await _educationService.UpdateAsync(id, education);
            if (updatedEducation == null)
                return NotFound(new ApiResponse<object>(false, "Education not found or already deleted"));
                
            return Ok(new ApiResponse<Education>(true, "Education updated successfully", updatedEducation));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating education {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while updating education"));
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _educationService.DeleteAsync(id);
            if (!result)
                return NotFound(new ApiResponse<object>(false, "Education not found or already deleted"));
                
            return Ok(new ApiResponse<object>(true, "Education deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting education {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while deleting education"));
        }
    }
}

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CertificatesController : ControllerBase
{
    private readonly ICertificateService _certificateService;
    private readonly ILogger<CertificatesController> _logger;
    
    public CertificatesController(ICertificateService certificateService, ILogger<CertificatesController> logger)
    {
        _certificateService = certificateService;
        _logger = logger;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var certificates = await _certificateService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<CertificateDto>>(true, "Certificates retrieved successfully", certificates));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving certificates");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving certificates"));
        }
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var certificate = await _certificateService.GetByIdAsync(id);
            if (certificate == null)
                return NotFound(new ApiResponse<object>(false, "Certificate not found"));
                
            return Ok(new ApiResponse<CertificateDto>(true, "Certificate retrieved successfully", certificate));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving certificate {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving certificate"));
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Create([FromBody] Certificate certificate)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var createdCertificate = await _certificateService.CreateAsync(certificate);
            return CreatedAtAction(nameof(GetById), new { id = createdCertificate.Id }, 
                new ApiResponse<Certificate>(true, "Certificate created successfully", createdCertificate));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating certificate");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while creating certificate"));
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Update(int id, [FromBody] Certificate certificate)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var updatedCertificate = await _certificateService.UpdateAsync(id, certificate);
            if (updatedCertificate == null)
                return NotFound(new ApiResponse<object>(false, "Certificate not found or already deleted"));
                
            return Ok(new ApiResponse<Certificate>(true, "Certificate updated successfully", updatedCertificate));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating certificate {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while updating certificate"));
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _certificateService.DeleteAsync(id);
            if (!result)
                return NotFound(new ApiResponse<object>(false, "Certificate not found or already deleted"));
                
            return Ok(new ApiResponse<object>(true, "Certificate deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting certificate {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while deleting certificate"));
        }
    }
}

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ReferencesController : ControllerBase
{
    private readonly IReferenceService _referenceService;
    private readonly ILogger<ReferencesController> _logger;
    
    public ReferencesController(IReferenceService referenceService, ILogger<ReferencesController> logger)
    {
        _referenceService = referenceService;
        _logger = logger;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var references = await _referenceService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ReferenceDto>>(true, "References retrieved successfully", references));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving references");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving references"));
        }
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var reference = await _referenceService.GetByIdAsync(id);
            if (reference == null)
                return NotFound(new ApiResponse<object>(false, "Reference not found"));
                
            return Ok(new ApiResponse<ReferenceDto>(true, "Reference retrieved successfully", reference));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reference {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving reference"));
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Create([FromBody] Reference reference)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var createdReference = await _referenceService.CreateAsync(reference);
            return CreatedAtAction(nameof(GetById), new { id = createdReference.Id }, 
                new ApiResponse<Reference>(true, "Reference created successfully", createdReference));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reference");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while creating reference"));
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Update(int id, [FromBody] Reference reference)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var updatedReference = await _referenceService.UpdateAsync(id, reference);
            if (updatedReference == null)
                return NotFound(new ApiResponse<object>(false, "Reference not found or already deleted"));
                
            return Ok(new ApiResponse<Reference>(true, "Reference updated successfully", updatedReference));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating reference {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while updating reference"));
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _referenceService.DeleteAsync(id);
            if (!result)
                return NotFound(new ApiResponse<object>(false, "Reference not found or already deleted"));
                
            return Ok(new ApiResponse<object>(true, "Reference deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting reference {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while deleting reference"));
        }
    }
}

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ServicesController : ControllerBase
{
    private readonly IServiceItemService _serviceService;
    private readonly ILogger<ServicesController> _logger;
    
    public ServicesController(IServiceItemService serviceService, ILogger<ServicesController> logger)
    {
        _serviceService = serviceService;
        _logger = logger;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var services = await _serviceService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ServiceItemDto>>(true, "Services retrieved successfully", services));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving services");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving services"));
        }
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound(new ApiResponse<object>(false, "Service not found"));
                
            return Ok(new ApiResponse<ServiceItemDto>(true, "Service retrieved successfully", service));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while retrieving service"));
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Create([FromBody] ServiceItem serviceItem)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var createdService = await _serviceService.CreateAsync(serviceItem);
            return CreatedAtAction(nameof(GetById), new { id = createdService.Id }, 
                new ApiResponse<ServiceItem>(true, "Service created successfully", createdService));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service");
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while creating service"));
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceItem serviceItem)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
        try
        {
            var updatedService = await _serviceService.UpdateAsync(id, serviceItem);
            if (updatedService == null)
                return NotFound(new ApiResponse<object>(false, "Service not found or already deleted"));
                
            return Ok(new ApiResponse<ServiceItem>(true, "Service updated successfully", updatedService));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while updating service"));
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _serviceService.DeleteAsync(id);
            if (!result)
                return NotFound(new ApiResponse<object>(false, "Service not found or already deleted"));
                
            return Ok(new ApiResponse<object>(true, "Service deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service {Id}", id);
            return StatusCode(500, new ApiResponse<object>(false, "An error occurred while deleting service"));
        }
    }
}
