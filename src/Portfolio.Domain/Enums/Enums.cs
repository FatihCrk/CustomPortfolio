namespace Portfolio.Domain.Enums;

/// <summary>
/// Kullanıcı rollerini tanımlar.
/// </summary>
public enum RoleType
{
    SuperAdmin = 1,
    Admin = 2,
    Editor = 3,
    Viewer = 4
}

/// <summary>
/// İletişim formu mesaj durumlarını tanımlar.
/// </summary>
public enum MessageStatus
{
    New = 1,
    Read = 2,
    Archived = 3,
    Replied = 4
}

/// <summary>
/// Blog yayın durumlarını tanımlar.
/// </summary>
public enum PublishStatus
{
    Draft = 1,
    Published = 2,
    Scheduled = 3
}

/// <summary>
/// Dosya türlerini tanımlar.
/// </summary>
public enum FileType
{
    Image = 1,
    Document = 2,
    Video = 3,
    Audio = 4,
    Other = 5
}

/// <summary>
/// Aktivite log tiplerini tanımlar.
/// </summary>
public enum ActivityType
{
    Login = 1,
    Logout = 2,
    FailedLogin = 3,
    Create = 4,
    Update = 5,
    Delete = 6,
    Export = 7,
    Import = 8,
    PasswordChange = 9,
    TokenRefresh = 10,
    SetupWizard = 11
}
