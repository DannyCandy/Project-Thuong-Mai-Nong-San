using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Models;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Areas.Admin.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/Statistics")]
    [Route("api/products")]
    public class ChartApiController : ControllerBase
    {
        private readonly AgriculturalProductsContext _context;
        public ChartApiController(AgriculturalProductsContext context)
        {
            _context = context;
        }

        // API: Lấy dữ liệu thống kê
        [HttpGet("GetVisitorData")]
        public IActionResult GetVisitorData([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = _context.VisitorStatistics.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(v => v.Date >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(v => v.Date <= endDate.Value);

            var data = query
                .OrderBy(v => v.Date)
                .Select(v => new
                {
                    Date = v.Date.ToString("yyyy-MM-dd"),
                    Count = v.VisitorCount
                }).ToList();

            return Ok(data);
        }


        // API: Thêm mới lượt truy cập
        [HttpPost("AddVisitor")]
        public async Task<IActionResult> AddVisitor()
        {
            var today = DateTime.Today;

            // Kiểm tra nếu người dùng đã truy cập hôm nay
            if (HttpContext.Session.GetString("VisitedToday") == null)
            {
                var todayData = await _context.VisitorStatistics.FirstOrDefaultAsync(v => v.Date == today);
                if (todayData != null)
                {
                    todayData.VisitorCount++;
                }
                else
                {
                    await _context.VisitorStatistics.AddAsync(new VisitorStatistic
                    {
                        Date = today,
                        VisitorCount = 1
                    });
                }
                await _context.SaveChangesAsync();

                // Đánh dấu người dùng đã truy cập hôm nay bằng cách lưu vào session
                HttpContext.Session.SetString("VisitedToday", "true");
            }

            return Ok();
        }
        //Xử lý tổng doanh thu theo tháng
        [HttpGet("total-price/{year}")]
        public async Task<IActionResult> GetTotalPriceByMonthInYear(int year)
        {
            var products = await _context.Orders
                .Where(o => o.OrderTime.Year == year)
                .GroupBy(o => new { o.OrderTime.Year, o.OrderTime.Month }) // Group by year and month
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalPrice = g.Sum(o => o.TotalPrice)
                })
                .ToListAsync();

            return Ok(products);
        }

        //Xử lý tổng số sản phẩm bán theo tháng trong năm
        [HttpGet("total-quantity/{year}")]
        public async Task<IActionResult> GetTotalQuantityByMonthInYear(int year)
        {
            var products = await _context.Orders
                .Where(o => o.OrderTime.Year == year)
                .Join(
                    _context.OrderDetails,
                    order => order.OrderId,
                    orderDetail => orderDetail.OrderId,
                    (order, orderDetail) => new { order, orderDetail }
                )
                .GroupBy(
                    x => new { x.order.OrderTime.Year, x.order.OrderTime.Month },
                    (key, group) => new
                    {
                        key.Year,
                        key.Month,
                        TotalQuantity = group.Sum(x => x.orderDetail.Quantity)
                    }
                )
                .ToListAsync();
            return Ok(products);
        }

    }
}

