using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly AgriculturalProductsContext _context;

        public EFCategoryRepository(AgriculturalProductsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        Dictionary<string, int> ICategoryRepository.GetTotalByCategory()
        {
            var productsByCategoryAndConfirmStatus = _context.SanPhams
            .Where(sp => sp.Confirm == true) // Lọc sản phẩm với confirm = true
            .GroupBy(sp => sp.Idcategory) // Nhóm sản phẩm theo tên category
            .Select(g => new { CategoryId = g.Key, ProductCount = g.Count() }) // Đếm số sản phẩm trong mỗi nhóm
            .ToDictionary(x => x.CategoryId, x => x.ProductCount); // Chuyển kết quả sang Dictionary

            return productsByCategoryAndConfirmStatus;
        }
    }

}
