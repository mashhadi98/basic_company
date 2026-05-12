namespace Company.Application.Authorization;

/// <summary>
/// نمایش خلاصهٔ یک مجوز برای APIهای مدیریتی یا Seed.
/// </summary>
public sealed record PermissionDto(int Id, string Name, string DisplayName, string? Description, string Group);
