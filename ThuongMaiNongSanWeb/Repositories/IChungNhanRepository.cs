using WebApp.Models;

namespace WebApp.Repositories
{
    public interface IChungNhanRepository
    {
        Task<IEnumerable<ChungNhan>> GetAllAsync();
        Task<ChungNhan> GetByIdAsync(string id);
        Task AddAsync(ChungNhan nhaPhanPhoi);
        Task UpdateAsync(ChungNhan nhaPhanPhoi);
        Task DeleteAsync(string id);
    }
}
