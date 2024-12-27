using System.ComponentModel.DataAnnotations;

namespace WebApp.MobileApp.DTO
{
    public class LoginModel
    {
        [Required]
        public string? UserNameOrEmail { get; set; }
        [Required] 
        public string? Password { get; set; } 
    }
}
