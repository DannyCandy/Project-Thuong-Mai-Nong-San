using System.ComponentModel.DataAnnotations;

namespace WebApp.MobileApp.DTO
{
    public class UpdateAvatarModel
    {
        [Required]
        public string IdUser { get; set; } = null!;

        [Required]
        public IFormFile Avatar { get; set; } = null!;
    }
}
