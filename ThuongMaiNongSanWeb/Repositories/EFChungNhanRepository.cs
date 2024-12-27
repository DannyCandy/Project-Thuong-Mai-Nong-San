using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class EFChungNhanRepository: IChungNhanRepository
    {
        private readonly AgriculturalProductsContext _context;
        public EFChungNhanRepository(AgriculturalProductsContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ChungNhan chungNhan)
        {
            _context.ChungNhans.Add(chungNhan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var chungNhan = await _context.ChungNhans.FindAsync(id);
            _context.ChungNhans.Remove(chungNhan);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChungNhan>> GetAllAsync()
        {
            return await _context.ChungNhans.Where(c => c.IsDeleted == false).ToListAsync();
        }

        public async Task<ChungNhan> GetByIdAsync(string id)
        {
            return await _context.ChungNhans.FindAsync(id);
        }

        public async Task UpdateAsync(ChungNhan chungNhan)
        {
            _context.ChungNhans.Update(chungNhan);
            await _context.SaveChangesAsync();
        }
    }
}
