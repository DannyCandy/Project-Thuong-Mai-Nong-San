using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISanPhamRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly AgriculturalProductsContext _context;

        public HomeController(ILogger<HomeController> logger,ISanPhamRepository productRepository, ICategoryRepository categoryRepository, AgriculturalProductsContext context)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, string category = "", string orderBy = "", string searchName = "")
        {
            //Xử lý phân trang cho sản phẩm
            var paginatedProduct = await _productRepository.GetProductByPaginated(8, pageIndex, category, orderBy, searchName);
            var isLogout = HttpContext.Session.GetObjectFromJson<string>("IsLogout") ?? "false";
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            ViewBag.IdCate = category;
            ViewBag.IsLogout = isLogout;
            return View(paginatedProduct);
        }
        public IActionResult ForbiddenPage()
        {
            return View();
        }
        public IActionResult ForbiddenPageForPost()
        {
        return View(); 
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Chức năng trở thành đại lý
        public async Task<IActionResult> AddAsync()
        {
            int fieldSetNum = 1;
            string idUser = User.FindFirst("UserId").Value ?? "";
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            if (DaiLy is not null)
            {
                if (DaiLy.Confirm)
                {
                    fieldSetNum = 4;
                }
                else
                {
                    fieldSetNum = 3;
                }
            }
            ViewBag.FieldSetNum = fieldSetNum;
            return View();
        }

        // Xử lý thêm sản phẩm mới

        [HttpPost]
        public async Task<IActionResult> Add(DaiLyOnline daiLy, IFormFile AvatarDaiLy)
        {
            string idUser = User.FindFirst("UserId").Value ?? "";
            daiLy.IdUser = idUser;
            daiLy.IdDaiLy = Guid.NewGuid().ToString();
            ModelState.Remove("IdDaiLy");
            ModelState.Remove("IdUser");
            if (ModelState.IsValid)
            {
                if (AvatarDaiLy is not null)
                {
                    // Lưu hình ảnh sản phẩm
                    daiLy.AvatarDaiLy = await SaveImage(AvatarDaiLy);
                }
                _context.DaiLyOnlines.Add(daiLy);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            // Nếu ModelState không hợp lệ, hiển thị lại form
            int fieldSetNum = 1;
            var DaiLy = await _context.DaiLyOnlines.FirstOrDefaultAsync(d => d.IdUser == idUser);
            if (DaiLy is not null)
            {
                if (DaiLy.Confirm)
                {
                    fieldSetNum = 4;
                }
                else
                {
                    fieldSetNum = 3;
                }
            }
            ViewBag.FieldSetNum = fieldSetNum;
            return View(daiLy);
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
