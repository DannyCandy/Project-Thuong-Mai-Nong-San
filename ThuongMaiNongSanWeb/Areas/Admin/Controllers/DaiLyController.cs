using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Repositories;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DaiLyController : Controller
    {
        private readonly AgriculturalProductsContext _context;
        public DaiLyController(AgriculturalProductsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var allUncheckAccount = await _context.DaiLyOnlines
                    .Where(dl => dl.Confirm == false) // Chỉ lấy các bản ghi có Confirm là false
                    .ToListAsync();
                return View(allUncheckAccount);
            }
            catch (Exception ex) {
                return NotFound(ex);
            }
        }

        public async Task<IActionResult> AcceptPermissionAsync(string id)
        {
            try
            {
                var daily = await _context.DaiLyOnlines.FindAsync(id);
                if (daily is null)
                {
                    return NotFound();
                }
                daily.Confirm = true;
                _context.DaiLyOnlines.Update(daily);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        
    }
}
