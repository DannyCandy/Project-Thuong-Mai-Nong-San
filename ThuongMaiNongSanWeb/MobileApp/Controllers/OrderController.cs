using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.MobileApp.DTO;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.MobileApp.Controllers
{
    [ApiController]
    [Route("api/app/[controller]/{action}")]
    public class OrderController : ControllerBase
    {
        private readonly AgriculturalProductsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(AgriculturalProductsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] OrderRequest orderObj , String addressId, string payment = "COD")
        {
            if (orderObj.Cart == null || !orderObj.Cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return NotFound();
            }
            if (payment == "COD")
            {
                try {
                    Order order = new Order();
                    var address = _context.ThongTinLienHes.FirstOrDefault(p => p.Idttlh == addressId);
                    if (address == null) return BadRequest();
                    var user = await _userManager.FindByIdAsync(address.UserId);
                    order.OrderId = Guid.NewGuid().ToString();
                    order.UserId = user.Id;
                    order.DcGiaoHang = address.DcgiaoHang;
                    order.Phone = address.Phone;
                    TimeZoneInfo timeZone = TimeZoneInfo.Local;
                    DateTime serverTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                    order.OrderTime = serverTime;

                    /*order.OrderTime = DateTime.UtcNow;*/
                    order.Message = orderObj.Message;
                    order.TotalPrice = orderObj.Cart.Items.Sum(i => i.Price * i.Quantity);
                    order.PaymentMethod = "COD";
                    order.Success = true;
                    order.OrderDetails = orderObj.Cart.Items.Select(i => new OrderDetail
                    {
                        OrderDetailId = Guid.NewGuid().ToString(),
                        Idsp = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList();

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    return Ok(order);
                }
                catch {
                    return BadRequest();
                }
            }
            return NotFound();
            /*else
            {
                string infoStr = $"{order.Message}|{order.DcGiaoHang}|{order.Phone}";
                var vnPayModel = new VnPaymentRequest
                {
                    Amount = cart.Items.Sum(i => i.Price * i.Quantity),
                    CreatedDate = DateTime.Now,
                    OrderId = "",
                    OrderInfo = infoStr,
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }*/
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetOrderHistory(string userId, int pageSize, int pageIndex, int month = -1, int year = -1)
        {

            IQueryable<Order> ordersQuery = null;
            if(month != -1 && year != -1)
            {
                ordersQuery = _context.Orders
                .AsNoTracking() 
                .Include(p => p.OrderDetails)
                .ThenInclude(od => od.IdspNavigation)
                .Where(p => p.UserId == userId && p.OrderTime.Month == month && p.OrderTime.Year == year)
                .OrderByDescending(p => p.OrderTime); // Sắp xếp sản phẩm theo ngày tăng dần
            }
            ordersQuery = _context.Orders
                .AsNoTracking()
                .Include(p => p.OrderDetails)
                .ThenInclude(od => od.IdspNavigation)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.OrderTime); // Sắp xếp sản phẩm theo ngày tăng dần

            if (ordersQuery is null) {
                return NotFound();
            }

            var paginatedOrders = await PaginatedList<Order>.CreateAsync(ordersQuery, pageIndex, pageSize);
            
            return Ok(paginatedOrders);
        }
    }
}
