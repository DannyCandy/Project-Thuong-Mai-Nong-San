using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Repositories
{
    public interface ISanPhamRepository
    {
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<SanPham> GetByIdAsync(string id);
        Task AddAsync(SanPham SanPham);
        Task UpdateAsync(SanPham SanPham);
        Task DeleteAsync(string id);
        Task<PaginatedList<SanPham>> GetProductByPaginated(int pageSize, int pageIndex, string category, string orderBy, string searchName);
        Task<PaginatedList<SanPham>> GetProductByPaginatedOfDaiLy(string idDaiLy,int pageSize, int pageIndex, string category, string orderBy, string searchName);

    }

}
