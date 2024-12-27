using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "CheckingAgency")]
    public class ProductController : Controller
    {
        private readonly ISanPhamRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        /*private readonly INhaPhanPhoiRepository _nhaPhanPhoiRepository;*/
        private readonly IChungNhanRepository _chungNhanRepository;
        private readonly AgriculturalProductsContext _context;

        public ProductController(ISanPhamRepository productRepository,
            ICategoryRepository categoryRepository, /*INhaPhanPhoiRepository nhaPhanPhoiRepository,*/
            IChungNhanRepository chungNhanRepository, AgriculturalProductsContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            /*_nhaPhanPhoiRepository = nhaPhanPhoiRepository;*/
            _chungNhanRepository = chungNhanRepository;
            _context = context;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            //Lấy id đại lý hiện tại
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            string idDaiLy = "";
            if (DaiLy is not null)
            {
                idDaiLy = DaiLy.IdDaiLy;
            }

            // Thêm kiểm tra trạng thái IsDeleted 

            /*var products = await _context.SanPhams.Where(sp => sp.IdDaiLy == idDaiLy).ToListAsync();*/

            var products = await _context.SanPhams
                             .Where(sp => sp.IdDaiLy == idDaiLy && !sp.IsDeleted)
                             .ToListAsync();

            var categories = await _categoryRepository.GetAllAsync();
            /*var nhaPhanPhois = await _nhaPhanPhoiRepository.GetAllAsync();*/
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            /* ViewBag.NhaPhanPhois = new SelectList(nhaPhanPhois, "Idnpp", "TenNpp");*/
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "HinhAnhChungNhan");
            ViewBag.DaiLys = DaiLy.NameDaiLy;
            return View(products);
        }

        // Hiển thị form thêm sản phẩm mới

        public async Task<IActionResult> AddAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            /*var nhaPhanPhois = await _nhaPhanPhoiRepository.GetAllAsync();*/
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            //Lấy id đại lý hiện tại
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            string idDaiLy = "";
            if (DaiLy is not null)
            {
                idDaiLy = DaiLy.IdDaiLy;
            }

            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            /*ViewBag.NhaPhanPhois = new SelectList(nhaPhanPhois, "Idnpp", "TenNpp");*/
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "IdchungNhan");
            ViewBag.DaiLys = idDaiLy;
            return View();
        }

        // Xử lý thêm sản phẩm mới

        [HttpPost]
        public async Task<IActionResult> Add(SanPham product, IFormFile HinhAnhSp)
        {
            product.Idsp = Guid.NewGuid().ToString();
            ModelState.Remove("Idsp");
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
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var categories = await _categoryRepository.GetAllAsync();
            /*var nhaPhanPhois = await _nhaPhanPhoiRepository.GetAllAsync();*/
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            //Lấy id đại lý hiện tại
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            string idDaiLy = "";
            if (DaiLy is not null)
            {
                idDaiLy = DaiLy.IdDaiLy;
            }

            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            /*ViewBag.NhaPhanPhois = new SelectList(nhaPhanPhois, "Idnpp", "TenNpp");*/
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "IdchungNhan");
            ViewBag.DaiLys = idDaiLy;
            return View(product);
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

        public async Task<IActionResult> Action()
        {
            var products = await _productRepository.GetAllAsync();
            if (products.Any())
            {
                return View(products);
            }
            return RedirectToAction("Page");
        }


        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            string daiLyName = "";
            if (DaiLy is not null)
            {
                daiLyName = DaiLy.NameDaiLy;
            }
            else
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            /*var nhaPhanPhois = await _nhaPhanPhoiRepository.GetAllAsync();*/
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            /*ViewBag.NhaPhanPhois = new SelectList(nhaPhanPhois, "Idnpp", "TenNpp");*/
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "HinhAnhChungNhan");
            ViewBag.DaiLyName = daiLyName;
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
            /*var nhaPhanPhois = await _nhaPhanPhoiRepository.GetAllAsync();*/
            var chungNhans = await _chungNhanRepository.GetAllAsync();
            //Lấy id đại lý hiện tại
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            string idDaiLy = "";
            if (DaiLy is not null)
            {
                idDaiLy = DaiLy.IdDaiLy;
            }
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            /*ViewBag.NhaPhanPhois = new SelectList(nhaPhanPhois, "Idnpp", "TenNpp");*/
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "IdchungNhan");
            ViewBag.DaiLys = idDaiLy;
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
            //Lấy id đại lý hiện tại
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            string idDaiLy = "";
            if (DaiLy is not null)
            {
                idDaiLy = DaiLy.IdDaiLy;
            }
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            ViewBag.ChungNhans = new SelectList(chungNhans, "IdchungNhan", "IdchungNhan");
            ViewBag.DaiLys = idDaiLy;
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

            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            string nameDaiLy = "";
            if (DaiLy is not null)
            {
                nameDaiLy = DaiLy.NameDaiLy;
            }
            ViewBag.DaiLyName = nameDaiLy;
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
    }
}
