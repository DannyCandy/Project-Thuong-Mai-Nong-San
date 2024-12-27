using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Repositories;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderHistory _orderHistory;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderController(IOrderHistory orderHistory, IOrderDetailRepository orderDetailRepository)
        {
            _orderHistory = orderHistory;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var listOrders = await _orderHistory.GetAllByAdmin();
                return View(listOrders);
            }
            catch (Exception ex)
            {
                return NotFound("Err message: " + ex.ToString());
            }
        }

        public async Task<IActionResult> DetailOrder(string idOrder)
        {
            try
            {
                var listDetailOrders = await _orderDetailRepository.GetAllByOrder(idOrder);
                ViewBag.OrderId = idOrder;
                return View(listDetailOrders);
            }
            catch (Exception ex)
            {
                return NotFound("Err message: " + ex.ToString());
            }
        }
    }
}