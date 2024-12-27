using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Areas.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly ISanPhamRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IChungNhanRepository _chungNhanRepository;
        private readonly IDaiLyRepository _daiLyRepository;
        private readonly AgriculturalProductsContext _context;

        public ProductController(ISanPhamRepository productRepository, ICategoryRepository categoryRepository,/* INhaPhanPhoiRepository nhaPhanPhoiRepository,*/ IChungNhanRepository chungNhanRepository, IDaiLyRepository daiLyRepository, AgriculturalProductsContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _chungNhanRepository = chungNhanRepository;
            _daiLyRepository = daiLyRepository;
            _context = context;
        }

        public HttpRequest GetRequest()
        {
            return Request;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index(int pageIndex = 1, string category = "", string orderBy = "", string searchName = "")
        {
            //Xử lý phân trang cho sản phẩm
            var paginatedProduct = await _productRepository.GetProductByPaginated(6, pageIndex, category, orderBy, searchName);

            var categories = await _categoryRepository.GetAllAsync();
            //Tổng sản phẩm theo từng category
            Dictionary<string, int> productCounts = _categoryRepository.GetTotalByCategory();
            ViewBag.ProductCounts = productCounts;
            ViewBag.OrderBy = orderBy;
            ViewBag.IdCate = category;
            ViewBag.Categories = new SelectList(categories, "Idcategory", "NameCategory");
            ViewBag.SearchName = searchName;
            ViewBag.IsNull = paginatedProduct.Count == 0 ? true : false;
            return View(paginatedProduct);
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

            //Tổng sản phẩm theo từng category
            Dictionary<string, int> productCounts = _categoryRepository.GetTotalByCategory();
            ViewBag.ProductCounts = productCounts;

            return View(product);
        }

        //Xử lý search sp
        public async Task<ActionResult> SearchSuggestions(string query)
        {
            var suggestions = await _context.SanPhams
                .Where(p => p.SpName.Contains(query) && p.Confirm && p.IsDeleted == false)
                .ToListAsync();
            return Ok(suggestions);
        }
    }
}
