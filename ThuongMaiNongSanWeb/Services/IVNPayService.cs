using WebApp.Models;

namespace WebApp.Services
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequest model);
        VNPaymentResponse PaymentExecute(IQueryCollection collections);
    }
}
