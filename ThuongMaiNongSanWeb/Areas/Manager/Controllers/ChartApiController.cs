using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Areas.Manager.Controllers
{
    [ApiController]
    /*[Authorize(Roles = "Manager")]*/
    [Route("api/productsDaiLy")]
    public class ChartApiController : ControllerBase
    {
        private readonly AgriculturalProductsContext _context;
        public ChartApiController(AgriculturalProductsContext context)
        {
            _context = context;
        }

        //Xử lý tổng doanh thu theo tháng
        [HttpGet("total-price/{year}/{id}")]
        public async Task<IActionResult> GetTotalPriceDaiLyByMonthInYear(int year, string id)
        {
            var products = await _context.OrderDetails
                .Where(od => od.IdspNavigation.IdDaiLy == id && od.OrderIdNavigation.OrderTime.Year == year)
                .Select(od => new
                {
                    Year = od.OrderIdNavigation.OrderTime.Year,
                    Month = od.OrderIdNavigation.OrderTime.Month,
                    TotalPrice = od.OrderIdNavigation.TotalPrice
                })
                .GroupBy(od => new { od.Year, od.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalPrice = g.Sum(x => x.TotalPrice)
                })
                .ToListAsync();


            return Ok(products);
        }

        //Xử lý tổng số sản phẩm bán theo tháng trong năm
        [HttpGet("total-quantity/{year}/{id}")]
        public async Task<IActionResult> GetTotalQuantityDaiLyByMonthInYear(int year, string id)
        {
            var quantitys = await _context.OrderDetails
                .Where(od => od.IdspNavigation.IdDaiLy == id && od.OrderIdNavigation.OrderTime.Year == year) // Điều kiện SanPham.IdDaiLy = id truyền vào
                .Join(_context.Orders, // Join với bảng Orders
                      od => od.OrderId,
                      o => o.OrderId,
                      (od, o) => new { OrderDetail = od, Order = o }) // Lấy ra OrderDetail và Order tương ứng
                .GroupBy(x => new { Year = x.Order.OrderTime.Year, Month = x.Order.OrderTime.Month }) // Gom nhóm theo Year và Month
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalQuantity = g.Sum(x => x.OrderDetail.Quantity) // Tính tổng Quantity
                })
                .ToListAsync();


            return Ok(quantitys);
        }
    }
}
