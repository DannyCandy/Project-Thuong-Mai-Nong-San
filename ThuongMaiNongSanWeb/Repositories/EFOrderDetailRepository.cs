using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class EFOrderDetailRepository: IOrderDetailRepository
    {
        private readonly AgriculturalProductsContext _context;
        public EFOrderDetailRepository(AgriculturalProductsContext context)
        {
            _context = context;
        }
        public async Task AddAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var OrderDetail = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(OrderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllByOrder(string id)
        {
            return await _context.OrderDetails.Include(dt => dt.IdspNavigation)
            .Where(dt => dt.OrderId == id).ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(string id)
        {
            return await _context.OrderDetails.FindAsync(id);
        }

        public async Task UpdateAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
