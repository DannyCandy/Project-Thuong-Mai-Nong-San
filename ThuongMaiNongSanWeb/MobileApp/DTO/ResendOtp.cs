using System.ComponentModel.DataAnnotations;

namespace WebApp.MobileApp.DTO
{
    public class ResendOtp
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}
