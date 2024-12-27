using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public partial class DaiLyOnline
    {
        public DaiLyOnline()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public string IdDaiLy { get; set; } = null!;
        public string NameDaiLy { get; set; } = null!;
        public string IdUser {  get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string GioiThieu { get; set; } = null!;
        public string? AvatarDaiLy { get; set; } = null!;
        public bool Confirm { get; set; } = false;
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
