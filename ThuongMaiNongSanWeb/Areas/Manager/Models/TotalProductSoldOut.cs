namespace WebApp.Areas.Manager.Models
{
    public class TotalProductSoldOut
    {
        public string TenSanPham { get; set; } = null!;
        public string MoTa { get; set; } = null!;
        public decimal Price { get; set; }
        public string LoaiSp { get; set; } = null!;
        public string HinhAnhSp { get; set; } = null!;
        public decimal TotalSolds { get; set; }
    }
}