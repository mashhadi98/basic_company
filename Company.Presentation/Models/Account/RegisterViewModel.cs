using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Models.Account;

public sealed class RegisterViewModel
{
    [Required(ErrorMessage = "ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل معتبر نیست.")]
    [Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "حداقل ۸ کاراکتر.")]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "تکرار رمز الزامی است.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "رمز و تکرار آن یکسان نیست.")]
    [Display(Name = "تکرار رمز عبور")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام الزامی است.")]
    [Display(Name = "نام")]
    [StringLength(128)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
    [Display(Name = "نام خانوادگی")]
    [StringLength(128)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره تماس الزامی است.")]
    [Phone(ErrorMessage = "فرمت شماره تماس معتبر نیست.")]
    [Display(Name = "شماره تماس")]
    [StringLength(32)]
    public string PhoneNumber { get; set; } = string.Empty;
}
