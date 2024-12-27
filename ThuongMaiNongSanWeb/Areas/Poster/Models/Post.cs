using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.Areas.Poster.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ApplicationUser Author { get; set; } = null!;
        public List<Image> Images { get; set; } = null!;// Danh sách hình ảnh của bài viết
    }
        public class Image
        {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;// Đường dẫn đến hình ảnh
        public int PostId { get; set; } // Khóa ngoại tham chiếu đến bài viết
        public Post Post { get; set; } = null!;// Bài viết liên kết với hình ảnh
    }
}
