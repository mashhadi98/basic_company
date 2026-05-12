using Company.Application.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Company.Presentation.Authorization;

/// <summary>
/// اعمال مجوز مبتنی بر نام (مثلاً Product.Create). معادل Authorize با Policy = Permission:نام.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permissionName)
    {
        Console.WriteLine(permissionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(permissionName);
        Policy = PermissionRequirement.PolicyPrefix + permissionName;
    }
}
