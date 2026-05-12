using Company.Application.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Company.Presentation.Authorization;

/// <summary>
/// بررسی وجود Claim مجوز با نام مشخص روی Principal جاری.
/// </summary>
public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User.HasClaim(AuthClaimTypes.Permission, requirement.PermissionName))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
