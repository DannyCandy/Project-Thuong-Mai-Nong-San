using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ISanPhamRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository, ISanPhamRepository productRepository)
        {

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var categoryList = await _categoryRepository.GetAllAsync();
            return View(categoryList);
        }

        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> AddAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            category.Idcategory = Guid.NewGuid().ToString();
            ModelState.Remove("Idcategory");
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                // Sau khi xử lý thành công, chuyển hướng đến action "Index" hoặc action khác tùy vào yêu cầu của bạn
                return RedirectToAction("Index");

            }
            return View(category);
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory", category.Idcategory);
            return View(category);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(string id, Category category)
        {
            if (id != category.Idcategory)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                category.IsDeleted = true;
                await _categoryRepository.UpdateAsync(category);
                //Xóa luôn product có IdCategory đã xóa
                var productList = await _productRepository.GetAllAsync();
                // Lấy danh sách sản phẩm có CategoryId = id
                var productListToDelete = productList.Where(p => p.Idcategory == id).ToList();
                // Xóa từng sản phẩm trong danh sách
                foreach (var product in productListToDelete)
                {
                    product.IsDeleted = true;
                    await _productRepository.UpdateAsync(product);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi xóa dữ liệu: " + ex.Message);
            }
        }
    }
}
