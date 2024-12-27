using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ResetPassword
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? Token { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Name = "Nhập mật khẩu mới")]
        public string? NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage = "Mật khẩu mới bạn nhập không trùng khớp!")]
        [Display(Name = "Nhập lại mật khẩu mới")]
        public string? ConfirmNewPassword { get; set; }

        public bool IsSuccess { get; set; }
    }
}
