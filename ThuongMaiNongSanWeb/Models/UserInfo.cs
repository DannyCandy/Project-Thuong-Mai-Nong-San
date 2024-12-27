namespace WebApp.Models
{
    public class UserInfo
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string TenKhachHang { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] Avatar { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = null!;

    }
}
