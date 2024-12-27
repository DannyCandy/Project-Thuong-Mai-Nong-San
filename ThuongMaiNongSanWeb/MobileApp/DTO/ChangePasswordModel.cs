using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApp.MobileApp.DTO
{
    public class ChangePasswordModel
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string CurrentPassword { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
