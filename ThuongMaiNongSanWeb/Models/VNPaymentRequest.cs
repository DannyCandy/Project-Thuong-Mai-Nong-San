namespace WebApp.Models
{
    public class VnPaymentRequest
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrderInfo { get; set; }
    }
}
