using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ISanPhamRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IChungNhanRepository _chungNhanRepository;
        private readonly IDaiLyRepository _daiLyRepository;

        public ProductController(ISanPhamRepository productRepository, ICategoryRepository categoryRepository, IChungNhanRepository chungNhanRepository, IDaiLyRepository daiLyRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _chungNhanRepository = chungNhanRepository;
            _daiLyRepository = daiLyRepository;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var productList = await _productRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "HinhAnhChungNhan");
            return View(productList);
        }

        //Xử lý lưu đường dẫn hình ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images/ProductImg", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/ProductImg/" + image.FileName; // Trả về đường dẫn tương đối
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");

            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "MoTa");
            ViewBag.ChungNhanHinhAnhs = new SelectList(chungNhans, "IdchungNhan", "HinhAnhChungNhan");
            return View(product);
        }

        // Hiển thị form cập nhật sản phẩm
        
        public async Task<IActionResult> Update(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            var daiLys = await _daiLyRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "IdchungNhan");
            ViewBag.DaiLys = new SelectList(daiLys, "IdDaiLy", "NameDaiLy");
            return View(product);
        }

        // Xử lý cập nhật sản phẩm
        
        [HttpPost]
        public async Task<IActionResult> Update(string id, SanPham product, IFormFile HinhAnhSp)
         {
            if (id != product.Idsp)
            {
                return BadRequest();
            }
            ModelState.Remove("Idsp");
            ModelState.Remove("HinhAnhSp");
            ModelState.Remove("IdcategoryNavigation");
            ModelState.Remove("IdchungNhanNavigation");
            ModelState.Remove("IdnppNavigation");
            ModelState.Remove("IddaiLyNavigation");
            if (ModelState.IsValid)
            {
                if (HinhAnhSp is not null)
                {
                    // Lưu hình ảnh sản phẩm
                    product.HinhAnhSp = await SaveImage(HinhAnhSp);
                }
                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            var daiLys = await _daiLyRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "IdchungNhan");
            ViewBag.DaiLys = new SelectList(daiLys, "IdDaiLy", "NameDaiLy");
            return View(product);
        }

        // Hiển thị form xác nhận xóa sản phẩm
        
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            //Lọc ra thông tin liên quan ứng với sán phẩm
            var categories = await _categoryRepository.GetAllAsync();
            var selectedCategory = categories.FirstOrDefault(c => c.Idcategory == product.Idcategory);
            ViewBag.CategoryName = selectedCategory?.NameCategory;

            var chungNhans = await _chungNhanRepository.GetAllAsync();
            var selectedChungNhan = chungNhans.FirstOrDefault(c => c.IdchungNhan == product.IdchungNhan);
            ViewBag.ChungNhanImg = selectedChungNhan?.HinhAnhChungNhan;

            var daiLys = await _daiLyRepository.GetAllAsync();
            var selectedDaiLy = daiLys.FirstOrDefault(c => c.IdDaiLy == product.IdDaiLy);
            ViewBag.DaiLyName = selectedDaiLy?.NameDaiLy;
            return View(product);
        }

        // Xử lý xóa sản phẩm

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var sp = await _productRepository.GetByIdAsync(id);
                sp.IsDeleted = true;
                await _productRepository.UpdateAsync(sp);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi xóa dữ liệu: " + ex.Message);
            }
        }

        public async Task<IActionResult> SoftDelete(string id)
        {
            try
            {
                var sp = await _productRepository.GetByIdAsync(id);
                sp.IsDeleted = true;
                await _productRepository.UpdateAsync(sp);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi xóa dữ liệu: " + ex.Message);
            }
        }
    }
}
