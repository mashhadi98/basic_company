using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Areas.Admin.Models.Users;

public sealed class UserCreateViewModel
{
    [Required(ErrorMessage = "نام کاربری الزامی است.")]
    [Display(Name = "نام کاربری")]
    [StringLength(64, MinimumLength = 3, ErrorMessage = "نام کاربری باید بین ۳ تا ۶۴ کاراکتر باشد.")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "نام کاربری فقط می‌تواند شامل حروف انگلیسی، عدد و کاراکترهای . _ - باشد.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل معتبر نیست.")]
    [Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "حداقل ۸ کاراکتر.")]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; } = string.Empty;

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

