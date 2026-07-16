namespace Portfolio.Domain.Enums;

public enum UserRole
{
    SuperAdmin = 0,
    Admin = 1,
    Editor = 2,
    Viewer = 3
}

public enum ContentType
{
    None = 0,
    Project = 1,
    Blog = 2,
    Service = 3,
    Testimonial = 4,
    Skill = 5,
    Experience = 6,
    Education = 7,
    Certificate = 8,
    ContactMessage = 9,
    SocialMedia = 10,
    SiteSetting = 11
}

public enum ThemeType
{
    Light = 0,
    Dark = 1
}
