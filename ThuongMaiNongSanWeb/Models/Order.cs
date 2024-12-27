using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public partial class Order
    {
        public string OrderId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        [Column(TypeName = "decimal(18,0)")]
        public decimal TotalPrice { get; set; }
        public DateTime OrderTime { get; set; }
        public string? Message { get; set; }
        public string? DcGiaoHang { get; set; }
        //Thêm thuộc tính
        public bool Success { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Phone {  get; set; }

        //Thêm khóa ngoại ánh xạ tới user
        [JsonIgnore]
        public ApplicationUser? User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        // Thêm thuộc tính để thiết lập mối quan hệ 1:N với VNPayment
        [JsonIgnore]
        public virtual ICollection<VNPaymentResponse> VNPayments { get; set; } 
    }
}
