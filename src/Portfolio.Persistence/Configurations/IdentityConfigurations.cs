using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Entities;

namespace Portfolio.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.ProfilePictureUrl).HasMaxLength(500);
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);
        builder.Property(u => u.TwoFactorSecret).HasMaxLength(256);
        builder.Property(u => u.LastLoginIp).HasMaxLength(45);
        builder.Property(u => u.LastLoginDevice).HasMaxLength(256);
        builder.Property(u => u.CreatedBy).HasMaxLength(50);
        builder.Property(u => u.UpdatedBy).HasMaxLength(50);
        builder.Property(u => u.DeletedBy).HasMaxLength(50);

        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ActivityLogs)
            .WithOne(al => al.User)
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.Sessions)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ApiKeys)
            .WithOne(ak => ak.User)
            .HasForeignKey(ak => ak.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
        builder.Property(r => r.Description).HasMaxLength(500);

        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => ur.Id);

        builder.Property(ur => ur.AssignedBy).HasMaxLength(50);

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Resource).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Action).IsRequired().HasMaxLength(50);

        builder.HasIndex(p => new { p.Resource, p.Action }).IsUnique();

        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(rp => rp.Id);

        builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token).IsRequired();
        builder.Property(rt => rt.ReplacedByToken).HasMaxLength(500);
        builder.Property(rt => rt.ReasonRevoked).HasMaxLength(500);

        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.HasIndex(rt => rt.UserId);
    }
}

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SessionToken).IsRequired();
        builder.Property(s => s.IpAddress).IsRequired().HasMaxLength(45);
        builder.Property(s => s.UserAgent).HasMaxLength(1000);
        builder.Property(s => s.DeviceInfo).HasMaxLength(256);
        builder.Property(s => s.BrowserInfo).HasMaxLength(100);
        builder.Property(s => s.OsInfo).HasMaxLength(100);

        builder.HasIndex(s => s.SessionToken).IsUnique();
        builder.HasIndex(s => s.UserId);
    }
}

public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.ToTable("ApiKeys");

        builder.HasKey(ak => ak.Id);

        builder.Property(ak => ak.Name).IsRequired().HasMaxLength(100);
        builder.Property(ak => ak.KeyHash).IsRequired();
        builder.Property(ak => ak.Prefix).HasMaxLength(20);

        builder.HasIndex(ak => ak.KeyHash).IsUnique();
        builder.HasIndex(ak => ak.UserId);
    }
}

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.ToTable("ActivityLogs");

        builder.HasKey(al => al.Id);

        builder.Property(al => al.ActivityType).IsRequired().HasMaxLength(50);
        builder.Property(al => al.Description).IsRequired().HasMaxLength(1000);
        builder.Property(al => al.EntityName).HasMaxLength(100);
        builder.Property(al => al.IpAddress).HasMaxLength(45);
        builder.Property(al => al.UserAgent).HasMaxLength(1000);
        builder.Property(al => al.DeviceInfo).HasMaxLength(256);
        builder.Property(al => al.BrowserInfo).HasMaxLength(100);
        builder.Property(al => al.ErrorMessage).HasMaxLength(2000);
        builder.Property(al => al.OldValues).HasMaxLength(-1);
        builder.Property(al => al.NewValues).HasMaxLength(-1);

        builder.HasIndex(al => al.UserId);
        builder.HasIndex(al => al.ActivityType);
        builder.HasIndex(al => al.CreatedAt);
    }
}

public class TokenBlacklistConfiguration : IEntityTypeConfiguration<TokenBlacklist>
{
    public void Configure(EntityTypeBuilder<TokenBlacklist> builder)
    {
        builder.ToTable("TokenBlacklists");

        builder.HasKey(tb => tb.Id);

        builder.Property(tb => tb.Token).IsRequired();
        builder.Property(tb => tb.Reason).HasMaxLength(500);

        builder.HasIndex(tb => tb.Token).IsUnique();
    }
}
