using WebApp.Models;

namespace WebApp.Repositories
{
    public interface IDaiLyRepository
    {
        Task<IEnumerable<DaiLyOnline>> GetAllAsync();
        Task<DaiLyOnline> GetByIdAsync(string id);
        Task AddAsync(DaiLyOnline DaiLyOnline);
        Task UpdateAsync(DaiLyOnline DaiLyOnline);
        Task DeleteAsync(string id);
    }
}
