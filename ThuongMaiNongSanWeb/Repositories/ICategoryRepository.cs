using WebApp.Models;

namespace WebApp.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(string id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(string id);
        Dictionary<string, int> GetTotalByCategory();
    }

}
