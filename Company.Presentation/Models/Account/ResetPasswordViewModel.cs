using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Models.Account;

public sealed class ResetPasswordViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    [MinLength(8)]
    [Display(Name = "رمز عبور جدید")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Display(Name = "تکرار رمز")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
