using WebApp.Models;

namespace WebApp.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task<OrderDetail> GetByIdAsync(string id);
        Task AddAsync(OrderDetail orderDetail);
        Task UpdateAsync(OrderDetail orderDetail);
        Task DeleteAsync(string id);
        Task<IEnumerable<OrderDetail>> GetAllByOrder(string id);
    }
}
