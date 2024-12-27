using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    
    public class ShoppingCartController : Controller
    {
        private readonly ISanPhamRepository _productRepository;
        private readonly AgriculturalProductsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVNPayService _vnPayService;
        public ShoppingCartController(AgriculturalProductsContext context, UserManager<ApplicationUser> userManager, ISanPhamRepository productRepository, IVNPayService vNPayService)
        {
            _context = context;
            _userManager = userManager;
            _productRepository = productRepository;
            _vnPayService = vNPayService;
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View(new Order());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order, string payment = "COD")
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index");
            }
            if (payment == "COD")
            {
                var user = await _userManager.GetUserAsync(User);
                order.OrderId = Guid.NewGuid().ToString();
                order.UserId = user.Id;
                //Lấy giờ máy chủ
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime serverTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                order.OrderTime = serverTime;
                /*order.OrderTime = DateTime.Now;*/
                order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
                order.PaymentMethod = "COD";
                order.Success = true;
                order.OrderDetails = cart.Items.Select(i => new OrderDetail
                {
                    OrderDetailId = Guid.NewGuid().ToString(),
                    Idsp = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList();

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("Cart");
                ViewBag.Order = order;

                return View("OrderCompleted"); // Trang xác nhận hoàn thành đơn hàng
            }
            else
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
            }
            
        }

        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var product = await GetProductFromDatabase(productId);

            var cartItem = new CartItem
            {
                ProductImg = product.HinhAnhSp,
                ProductId = productId,
                Name = product.SpName,
                Price = product.Price,
                Quantity = quantity
            };

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
        }

        
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            if(cart.Items.Count > 0)
            {
                return View("Index",cart);
            }
            return View("EmptyCart");
        }

        // Các actions khác...

        private async Task<SanPham> GetProductFromDatabase(string productId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }

        //Update số lượng và đơn giá
        public ActionResult UpdateCart(string productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            cart.UpdateItem(productId, quantity);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            var cartItem = cart.GetItem(productId);

            return Json(new { success = true, price = cartItem.Price, quantity = cartItem.Quantity });
        }

        public IActionResult RemoveFromCart(string productId)
        {
            //Xóa cartItem từ giỏ hàng
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

            if (cart is not  null)
            {
                cart.RemoveItem(productId);

                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        //Xử lý liên quan tới VNPay

        
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> PaymentCallBack()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index");
            }

            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            // Lưu đơn hàng vô database
            // Lưu vào bảng Order
            var user = await _userManager.GetUserAsync(User);
            Order order = new Order();
            order.OrderId = Guid.NewGuid().ToString();
            order.UserId = user.Id;
            order.OrderTime = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.PaymentMethod = "VNPAY";
            order.Success = true;
            //Convert orderInfo
            var parts = response.OrderDescription.Split('|');
            order.Message = parts[0];
            order.DcGiaoHang = parts[1];
            order.Phone = parts[2];
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                OrderDetailId = Guid.NewGuid().ToString(),
                Idsp = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            // Lưu vào bảng GiaoDichVnPay
            response.OrderId = order.OrderId;
            response.OrderDescription = "Thanh toán cho đơn hàng: " + response.OrderId;
            _context.VNPaymentResponses.Add(response);

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            ViewBag.Order = order;

            return View("OrderCompleted"); // Trang xác nhận hoàn thành đơn hàng
        }

        //Xử lý sử dụng địa chỉ mặc định
        public async Task<ActionResult> UsingDefaultAddress()
        {
            var user = await _userManager.GetUserAsync(User);
            var defaultAddress = await _context.ThongTinLienHes
                .Where(p => p.UserId == user.Id && p.DefaultPos)
                .FirstOrDefaultAsync();
            return Ok(defaultAddress);
        }
    }
}
