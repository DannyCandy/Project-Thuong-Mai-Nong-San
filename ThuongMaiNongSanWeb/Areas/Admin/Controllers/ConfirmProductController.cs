using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ConfirmProductController : Controller
    {
        private readonly AgriculturalProductsContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IChungNhanRepository _chungNhanRepository;
        private readonly ISanPhamRepository _sanPhamRepository;
        public ConfirmProductController(AgriculturalProductsContext context, ICategoryRepository categoryRepository, IChungNhanRepository chungNhanRepository, ISanPhamRepository sanPhamRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _chungNhanRepository = chungNhanRepository;
            _sanPhamRepository = sanPhamRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var totalProductsByDaiLy = await _context.DaiLyOnlines
                    .Where(dl => dl.Confirm == true)
                    .Select(dl => new
                    {
                        DaiLyName = dl.NameDaiLy,
                        TotalProducts = _context.SanPhams.Count(sp => sp.IdDaiLy == dl.IdDaiLy && sp.Confirm == false && sp.IsDeleted == false),
                        DaiLyId = dl.IdDaiLy,
                    })
                    .ToListAsync();
                List<TotalProductOfDaiLy> totalProductsList = new List<TotalProductOfDaiLy>();

                // Duyệt qua từng phần tử trong danh sách danh sách các đối tượng không tên và gán giá trị cho từng thuộc tính của đối tượng mô hình
                foreach (var item in totalProductsByDaiLy)
                {
                    TotalProductOfDaiLy totalProductOfDaiLy = new TotalProductOfDaiLy
                    {
                        DaiLyName = item.DaiLyName,
                        TotalProducts = item.TotalProducts,
                        DaiLyId = item.DaiLyId
                    };
                    TotalProductOfDaiLy t = totalProductOfDaiLy;

                    // Thêm đối tượng mô hình vào danh sách mô hình
                    totalProductsList.Add(t);
                }

                return View(totalProductsList);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        public async Task<IActionResult> CheckProduct(string id)
        {
            try
            {
                var products = await _context.SanPhams
                    .Where(sp => sp.IdDaiLy == id && sp.Confirm == false && sp.IsDeleted == false)
                    .ToListAsync();

                if (products is null || !products.Any())
                {
                    return RedirectToAction(nameof(Index));
                }
                var categories = await _categoryRepository.GetAllAsync();
                var chungNhans = await _chungNhanRepository.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
                ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "HinhAnhChungNhan");

                return View(products);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        public async Task<IActionResult> SetConfirm(string id)
        {
            try
            {
                var products = await _sanPhamRepository.GetByIdAsync(id);
                if (products is null)
                {
                    return NotFound();
                }

                products.Confirm = true;
                await _sanPhamRepository.UpdateAsync(products);

                return RedirectToAction(nameof(CheckProduct), new { id = products.IdDaiLy });
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        public async Task<IActionResult> SetNoConfirm(string id)
        {
            try
            {
                var products = await _sanPhamRepository.GetByIdAsync(id);
                if (products is null)
                {
                    return NotFound();
                }
                products.IsDeleted = true;
                await _sanPhamRepository.UpdateAsync(products);
                return RedirectToAction(nameof(CheckProduct), new { id = products.IdDaiLy });
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        
    }
}
