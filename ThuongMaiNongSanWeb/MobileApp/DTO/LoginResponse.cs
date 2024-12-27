using Microsoft.AspNetCore.Mvc;

namespace WebApp.MobileApp.DTO
{
    public class LoginResponse : Status
    {
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
        public string? UserAvatar {  get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Diachi {  get; set; }
    }
}
