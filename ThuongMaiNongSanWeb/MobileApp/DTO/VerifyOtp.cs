using System.ComponentModel.DataAnnotations;
namespace WebApp.MobileApp.DTO
{
    public class VerifyOtp
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Otp { get; set; } = null!;
    }
}
