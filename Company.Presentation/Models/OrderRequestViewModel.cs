using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Models
{
    public class OrderRequestViewModel
    {
        [Required(ErrorMessage = "شماره تماس الزامی است.")]
        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "نام و نام خانوادگی")]
        public string? FullName { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }
}
