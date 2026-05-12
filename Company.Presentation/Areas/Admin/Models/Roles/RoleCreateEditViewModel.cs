using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Areas.Admin.Models.Roles;

public sealed class RoleCreateEditViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "نام نقش الزامی است.")]
    [Display(Name = "نام نقش")]
    [StringLength(64, MinimumLength = 2, ErrorMessage = "نام نقش باید بین ۲ تا ۶۴ کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "توضیح")]
    [StringLength(256)]
    public string? Description { get; set; }
}

