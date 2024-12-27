using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Manager.Models;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class EFOrderHistory : IOrderHistory
    {
        private readonly AgriculturalProductsContext _context;

        public EFOrderHistory(AgriculturalProductsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllByAdmin()
        {
            return await _context.Orders
            .ToListAsync();
        }

        public async Task<IEnumerable<TotalProductSoldOut>> GetAllByDaiLy(string idDaiLy)
        {
            var totalProductsSold = await _context.SanPhams
        .Include(sp => sp.IdcategoryNavigation)
        .Where(sp => sp.Confirm == true && sp.IdDaiLy == idDaiLy)
        .Select(p => new
        {
            TenSanPham = p.SpName,
            MoTa = p.MoTa,
            Price = p.Price,
            LoaiSp = p.IdcategoryNavigation.NameCategory,
            HinhAnhSp = p.HinhAnhSp,
            TotalProducts = _context.OrderDetails
            .Include(p => p.OrderIdNavigation)
            .Where(od => od.Idsp == p.Idsp && od.OrderIdNavigation.OrderTime.Year == 2024)
            .Sum(od => od.Quantity),
        })
        .ToListAsync();
            List<TotalProductSoldOut> totalProductsList = new List<TotalProductSoldOut>();

            // Duyệt qua từng phần tử trong danh sách danh sách các đối tượng không tên và gán giá trị cho từng thuộc tính của đối tượng mô hình
            foreach (var item in totalProductsSold)
            {
                TotalProductSoldOut totalProductOfDaiLy = new TotalProductSoldOut
                {
                    TenSanPham = item.TenSanPham,
                    MoTa = item.MoTa,
                    Price = item.Price,
                    LoaiSp = item.LoaiSp,
                    HinhAnhSp = item.HinhAnhSp,
                    TotalSolds = item.TotalProducts
                };
                TotalProductSoldOut t = totalProductOfDaiLy;

                // Thêm đối tượng mô hình vào danh sách mô hình
                totalProductsList.Add(t);
            }
            return totalProductsList;
        }

        public async Task<PaginatedList<Order>> GetOrderByPaginated(string userId,int pageSize, int pageIndex, int month = -1, int year = -1)
        {

            IQueryable<Order> ordersQuery = null;
            ordersQuery = _context.Orders
                .Include(p => p.OrderDetails)
                .ThenInclude(od => od.IdspNavigation)
                .Where(p => p.UserId == userId && p.OrderTime.Month == month && p.OrderTime.Year == year)
                .OrderBy(p => p.OrderTime); // Sắp xếp sản phẩm theo ngày tăng dần
            
            var paginatedOrders = await PaginatedList<Order>.CreateAsync(ordersQuery,pageIndex, pageSize);
            return paginatedOrders;
        }
    }
}
