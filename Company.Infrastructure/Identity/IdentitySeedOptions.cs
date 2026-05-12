namespace Company.Infrastructure.Identity;

/// <summary>
/// تنظیمات Seed اولیهٔ ادمین از appsettings (نام کاربری همان ایمیل است).
/// </summary>
public sealed class IdentitySeedOptions
{
    public const string SectionName = "IdentitySeed";

    public string AdminEmail { get; set; } = "admin@company.local";
    public string AdminPassword { get; set; } = "Admin@123456";
    public string AdminPhoneNumber { get; set; } = "+989000000000";
    public string AdminFirstName { get; set; } = "Admin";
    public string AdminLastName { get; set; } = "User";

    /// <summary>نام نقش ادمین.</summary>
    public string AdminRoleName { get; set; } = "Admin";
}
