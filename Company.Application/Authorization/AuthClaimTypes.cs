namespace Company.Application.Authorization;

/// <summary>
/// نوع Claim برای ذخیره نام مجوزها در کوکی احراز هویت.
/// </summary>
public static class AuthClaimTypes
{
    /// <summary>مقدار Claim برابر نام مجوز است (مثلاً Product.Create).</summary>
    public const string Permission = "permission";
}
