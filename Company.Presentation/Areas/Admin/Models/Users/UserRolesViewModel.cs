using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Areas.Admin.Models.Users;

public sealed class UserRolesViewModel
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public IReadOnlyList<RoleItem> AllRoles { get; set; } = Array.Empty<RoleItem>();

    public HashSet<string> AssignedRoles { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public sealed record RoleItem(string Name, string? Description);
}

