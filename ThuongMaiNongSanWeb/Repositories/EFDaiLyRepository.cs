using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class EFDaiLyRepository: IDaiLyRepository
    {
        private readonly AgriculturalProductsContext _context;

        public EFDaiLyRepository(AgriculturalProductsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DaiLyOnline>> GetAllAsync()
        {
            return await _context.DaiLyOnlines.ToListAsync();
        }

        public async Task<DaiLyOnline> GetByIdAsync(string id)
        {
            return await _context.DaiLyOnlines.FindAsync(id);
        }

        public async Task AddAsync(DaiLyOnline DaiLyOnlines)
        {
            _context.DaiLyOnlines.Add(DaiLyOnlines);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DaiLyOnline DaiLyOnlines)
        {
            _context.DaiLyOnlines.Update(DaiLyOnlines);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var DaiLyOnlines = await _context.DaiLyOnlines.FindAsync(id);
            _context.DaiLyOnlines.Remove(DaiLyOnlines);
            await _context.SaveChangesAsync();
        }
    }
}
