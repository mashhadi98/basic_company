using Company.Infrastructure.Persistence.Identity;

namespace Company.Presentation.Areas.Admin.Models.Users;

public sealed record UserListItemViewModel(
    string Id,
    string UserName,
    string Email,
    string FullName,
    string PhoneNumber,
    bool EmailConfirmed,
    bool PhoneNumberConfirmed)
{
    public static UserListItemViewModel FromUser(ApplicationUser user) =>
        new(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            user.FullName,
            user.PhoneNumber ?? string.Empty,
            user.EmailConfirmed,
            user.PhoneNumberConfirmed);
}

