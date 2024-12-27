using MailKit.Search;
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
    public class ManagerController : Controller
    {
        private readonly AgriculturalProductsContext _context;
        private readonly ISanPhamRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderHistory _orderHistory;
        private readonly IDaiLyRepository _daiLyRepository;
        public ManagerController(AgriculturalProductsContext context, ISanPhamRepository productRepository, ICategoryRepository categoryRepository, IOrderHistory orderHistory, IDaiLyRepository daiLyRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderHistory = orderHistory;
            _daiLyRepository = daiLyRepository;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser && d.Confirm);
            string idDaiLy = "";
            if (DaiLy is not null)
            {
                idDaiLy = DaiLy.IdDaiLy;
            }
            else
            {
                return RedirectToAction("ForbiddenPage", "Home", new { area = "" });
            }
            ViewBag.NameDaiLy = DaiLy.NameDaiLy;
            ViewBag.IdDaiLy = idDaiLy;
            ViewBag.IdUser = idUser;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Home(int pageIndex = 1, string category = "", string orderBy = "", string searchName = "")
        {
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser && d.Confirm);
            string idDaiLy = "";
            if (DaiLy is not null)
            {
                idDaiLy = DaiLy.IdDaiLy;
            }
            else
            {
                return RedirectToAction("ForbiddenPage", "Home", new { area = "" });
            }
            var categories = await _categoryRepository.GetAllAsync();
            // Đếm số sản phẩm liên quan đến DaiLy đã tìm thấy và có Confirm == false
            var totalProducts = await _context.SanPhams
                .CountAsync(sp => sp.IdDaiLy == idDaiLy && sp.Confirm == true);
            // Tạo đối tượng kết quả với tổng số sản phẩm
            ViewBag.IdDaiLy = idDaiLy;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.NameDaiLy = DaiLy.NameDaiLy;
            ViewBag.MotaDaiLy = DaiLy.GioiThieu;
            ViewBag.OrderBy = orderBy;
            ViewBag.IdCate = category;
            ViewBag.AvatarDaiLy = DaiLy.AvatarDaiLy ?? "";
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            ViewBag.SearchName = searchName;
            // Phân trang sản phẩm theo đại lý
            var paginatedProduct = await _productRepository.GetProductByPaginatedOfDaiLy(idDaiLy,9, pageIndex, category, orderBy, searchName);
            ViewBag.IsNull = paginatedProduct.Count == 0 ? true : false;
            return View(paginatedProduct);
        }

        //Xử lý search sp
        public async Task<ActionResult> SearchSuggestions(string query, string idDaiLy)
        {
            var suggestions = await _context.SanPhams
                .Where(p => p.SpName.Contains(query) && p.Confirm && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                .ToListAsync();
            return Ok(suggestions);
        }
        [Authorize]
        public async Task<IActionResult> SoldProducts()
        {
            try
            {
                string idUser = User.FindFirst("UserId").Value ?? "";
                var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser && d.Confirm);
                string idDaiLy = DaiLy.IdDaiLy;
                var listSoldProducts = await _orderHistory.GetAllByDaiLy(idDaiLy);
                return View(listSoldProducts);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ForbiddenPage", "Home", new { area = "" });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeAvt(IFormFile avtDaiLy)
        {
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser && d.Confirm);
            if (DaiLy is null)
            {
                return RedirectToAction("ForbiddenPage", "Home", new { area = "" });
            }
            if (avtDaiLy is not null)
            {
                // Lưu hình ảnh sản phẩm
                DaiLy.AvatarDaiLy = await SaveImage(avtDaiLy);
                await _daiLyRepository.UpdateAsync(DaiLy);
            }
            return RedirectToAction(nameof(Home));
        }

        //Xử lý lưu đường dẫn hình ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images/DaiLyImg", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/DaiLyImg/" + image.FileName; // Trả về đường dẫn tương đối
        }
    }
}

