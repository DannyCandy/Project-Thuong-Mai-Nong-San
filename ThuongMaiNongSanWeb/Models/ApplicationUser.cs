using Microsoft.AspNetCore.Identity;
using WebApp.Areas.Poster.Models;

namespace WebApp.Models
{
    public class ApplicationUser: IdentityUser
    {
        public override string UserName { get; set; } = null!;
        public string TenKhachHang { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public byte[] Avatar { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = null!;
    }
}
