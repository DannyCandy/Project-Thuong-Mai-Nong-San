using System.ComponentModel.DataAnnotations;

namespace WebApp.MobileApp.DTO
{
    public class UpdateUserInfo
    {
        [Required]
        public string? UserId { get; set; } 
        public string? UserName { get; set; }
        public string? TenKhachHang { get; set; } 
        public string? Phone { get; set; } 
        public string? DiaChi { get; set; } 
    }
}
