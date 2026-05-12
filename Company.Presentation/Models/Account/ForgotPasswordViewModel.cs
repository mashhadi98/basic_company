using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Models.Account;

public sealed class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "ایمیل الزامی است.")]
    [EmailAddress]
    [Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;
}
