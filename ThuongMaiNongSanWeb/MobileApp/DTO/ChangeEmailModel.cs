using System.ComponentModel.DataAnnotations;

namespace WebApp.MobileApp.DTO
{
    public class ChangeEmailModel
    {
        [Required]
        public string userId { get; set; } = null!;
        public string? Email { get; set; }
    }
}
