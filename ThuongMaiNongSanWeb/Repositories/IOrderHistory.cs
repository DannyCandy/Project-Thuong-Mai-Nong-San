using WebApp.Areas.Manager.Models;
using WebApp.Models;

namespace WebApp.Repositories
{
    public interface IOrderHistory
    {
        public Task<PaginatedList<Order>> GetOrderByPaginated(string userId,int pageSize, int pageIndex, int month, int year);
        public Task<IEnumerable<Order>> GetAllByAdmin();
        public Task<IEnumerable<TotalProductSoldOut>> GetAllByDaiLy(string idDaiLy);
    }
}
