using WebApp.Models;

namespace WebApp.MobileApp.Models
{
    public class TokenInfo
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Usename { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? VerifyOtp { get; set; }
        public DateTime? VerifyTokenExpiry { get; set;}
        //Đã lấy otp từ email đăng kí
        public bool EmailConfirmed { get; set; } = false;
        //Ngày kích hoạt account
        public DateTime? ActiveDate { get; set; }
        //Lấy lại mật khẩu
        public string? ResetPasswordOtp { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public string? ResetPasswordToken {  get; set; }
        public virtual ApplicationUser? IdUserNavigation { get; set; } = null!;
    }
}
