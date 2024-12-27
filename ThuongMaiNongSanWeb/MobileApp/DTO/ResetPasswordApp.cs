using System.ComponentModel.DataAnnotations;

namespace WebApp.MobileApp.DTO
{
    public class ResetPasswordApp
    {
        [Required]
        public string? Otp { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string? NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "Nhập lại mật khẩu mới")]
        public string? ConfirmNewPassword { get; set; }
    }
}
