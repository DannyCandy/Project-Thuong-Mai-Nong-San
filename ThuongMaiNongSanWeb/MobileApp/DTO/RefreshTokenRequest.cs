using Microsoft.AspNetCore.Mvc;

namespace WebApp.MobileApp.DTO
{
    public class RefreshTokenRequest
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
