using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp.MobileApp.DTO
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string TenKhachHang { get; set; } = null!;
        [Required]
        public string DiaChi { get; set; } = null!;
        [Required]
        public IFormFile Avatar { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
    }
}
