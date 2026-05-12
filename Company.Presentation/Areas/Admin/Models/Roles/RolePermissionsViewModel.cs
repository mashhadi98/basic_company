using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Areas.Admin.Models.Roles;

public sealed class RolePermissionsViewModel
{
    [Required]
    public string RoleId { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;

    public IReadOnlyList<PermissionItem> AllPermissions { get; set; } = Array.Empty<PermissionItem>();

    public HashSet<string> AssignedPermissionNames { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public sealed record PermissionItem(int Id, string Name, string DisplayName, string Group);
}

