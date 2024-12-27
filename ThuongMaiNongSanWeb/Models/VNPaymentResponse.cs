namespace WebApp.Models
{
    public class VNPaymentResponse
    {
        public string TransactionId { get; set; }
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
        //Thiết lập quan hệ với Order
        public virtual Order OrderIdNavigation { get; set; } = null!;
    }
}
