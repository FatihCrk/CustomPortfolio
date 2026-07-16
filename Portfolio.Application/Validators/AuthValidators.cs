using FluentValidation;
using Portfolio.Application.DTOs;

namespace Portfolio.Application.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MinimumLength(2).WithMessage("Full name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");
            
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers and underscores");
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]").WithMessage("Password must contain uppercase, lowercase, number and special character");
            
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}

public class SetupAdminDtoValidator : AbstractValidator<SetupAdminDto>
{
    public SetupAdminDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MinimumLength(2).WithMessage("Full name must be at least 2 characters");
            
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters");
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]").WithMessage("Password must contain uppercase, lowercase, number and special character");
    }
}

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(2).WithMessage("Title must be at least 2 characters")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");
            
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters");
            
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
            
        RuleForEach(x => x.ImageUrls)
            .NotEmpty().WithMessage("At least one image is required");
    }
}

public class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Skill name is required")
            .MinimumLength(2).WithMessage("Skill name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Skill name cannot exceed 100 characters");
            
        RuleFor(x => x.Percentage)
            .InclusiveBetween(0, 100).WithMessage("Percentage must be between 0 and 100");
            
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}

public class CreateExperienceDtoValidator : AbstractValidator<CreateExperienceDto>
{
    public CreateExperienceDtoValidator()
    {
        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters");
            
        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Position is required")
            .MaximumLength(200).WithMessage("Position cannot exceed 200 characters");
            
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required");
            
        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");
            
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be after start date");
    }
}

public class CreateEducationDtoValidator : AbstractValidator<CreateEducationDto>
{
    public CreateEducationDtoValidator()
    {
        RuleFor(x => x.School)
            .NotEmpty().WithMessage("School name is required")
            .MaximumLength(200).WithMessage("School name cannot exceed 200 characters");
            
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required")
            .MaximumLength(200).WithMessage("Department cannot exceed 200 characters");
            
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required");
            
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be after start date");
    }
}

public class CreateMessageDtoValidator : AbstractValidator<CreateMessageDto>
{
    public CreateMessageDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(200).WithMessage("Subject cannot exceed 200 characters");
            
        RuleFor(x => x.MessageContent)
            .NotEmpty().WithMessage("Message is required")
            .MinimumLength(10).WithMessage("Message must be at least 10 characters")
            .MaximumLength(5000).WithMessage("Message cannot exceed 5000 characters");
    }
}

public class CreateBlogPostDtoValidator : AbstractValidator<CreateBlogPostDto>
{
    public CreateBlogPostDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(5).WithMessage("Title must be at least 5 characters")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");
            
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MinimumLength(50).WithMessage("Content must be at least 50 characters");
            
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
            
        RuleFor(x => x.MetaDescription)
            .MaximumLength(160).WithMessage("Meta description cannot exceed 160 characters");
    }
}

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required");
            
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]").WithMessage("Password must contain uppercase, lowercase, number and special character");
            
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
    }
}

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]").WithMessage("Password must contain uppercase, lowercase, number and special character");
    }
}
