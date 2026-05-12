using Company.Infrastructure.Persistence.Identity;

namespace Company.Presentation.Areas.Admin.Models.Roles;

public sealed record RoleListItemViewModel(string Id, string Name, string? Description)
{
    public static RoleListItemViewModel FromRole(ApplicationRole role) =>
        new(role.Id, role.Name ?? string.Empty, role.Description);
}

