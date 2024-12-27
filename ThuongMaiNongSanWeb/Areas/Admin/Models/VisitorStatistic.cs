namespace WebApp.Areas.Admin.Models
{
    public class VisitorStatistic
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } // Ngày truy cập
        public int VisitorCount { get; set; } // Số lượt truy cập
    }
}
