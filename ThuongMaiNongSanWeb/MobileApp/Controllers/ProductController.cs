using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.MobileApp.Controllers
{
    [ApiController]
    [Route("api/app/[controller]/{action}")]
    public class ProductController : ControllerBase
    {
        private readonly AgriculturalProductsContext _context;

        public ProductController(AgriculturalProductsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByPaginated(int pageSize, int pageIndex, string? category = "", string? orderBy = "", string? searchName = "")
        {
            if (string.IsNullOrEmpty(searchName))
            {
                if (string.IsNullOrEmpty(category))
                {
                    IQueryable<SanPham>? productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IsDeleted == false);
                    }
                    if(productsQuery is null)
                    {
                        return NotFound();
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return Ok(paginatedProducts);
                }
                else
                {
                    IQueryable<SanPham>? productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IsDeleted == false);
                    }
                    if (productsQuery is null)
                    {
                        return NotFound();
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return Ok(paginatedProducts);
                }
            }
            else
            {
                searchName = searchName.ToLower();
                if (string.IsNullOrEmpty(category))
                {
                    IQueryable<SanPham>? productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false);
                    }
                    if (productsQuery is null)
                    {
                        return NotFound();
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return Ok(paginatedProducts);
                }
                else
                {
                    IQueryable<SanPham>? productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams.AsNoTracking()
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false);
                    }
                    if (productsQuery is null)
                    {
                        return NotFound();
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return Ok(paginatedProducts);
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var listProduct = await _context.SanPhams
                .Where(sp => sp.IsDeleted == false).AsNoTracking()
                .Select(p => new
                {
                    p.Idsp,
                    p.SpName,
                    p.HinhAnhSp,
                    p.Price,
                    CategoryName = p.IdcategoryNavigation.NameCategory,
                })
                .ToListAsync();
            return Ok(listProduct);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(string id)
        {
            var sp = await _context.SanPhams
                .AsNoTracking() // Thêm AsNoTracking() để không theo dõi các thực thể
                .Include(p => p.IdcategoryNavigation)
                .Include(p => p.IddaiLyNavigation)
                .Include(p => p.IdchungNhanNavigation)
                .AsNoTracking()
                .Where(p => p.Idsp == id)
                .Select(p => new
                {
                    p.Idsp,
                    p.SpName,
                    p.MoTa,
                    p.CongDung,
                    p.ThanhPhanDinhDuong,
                    p.Hdsd,
                    p.SoLuongSp,
                    p.HinhAnhSp,
                    p.Price,
                    CategoryName = p.IdcategoryNavigation.NameCategory,
                    p.IddaiLyNavigation.IdDaiLy,
                    DaiLyName = p.IddaiLyNavigation.NameDaiLy,
                    MoTaCN = p.IdchungNhanNavigation.MoTa,
                    HinhAnhCN = p.IdchungNhanNavigation.HinhAnhChungNhan,
                })
                .FirstOrDefaultAsync();
            if (sp == null)
            {
                return NotFound();
            }
            return Ok(sp);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var listProduct = await _context.Categories
                .Where(sp => sp.IsDeleted == false).AsNoTracking()
                .Select(p => new
                {
                    p.Idcategory,
                    p.NameCategory,
                })
                .ToListAsync();
            return Ok(listProduct);
        }

        /*[HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchSuggestions(string query)
        {
            var suggestions = await _context.SanPhams
                .Where(p => p.SpName.Contains(query) && p.Confirm && p.IsDeleted == false)
                .AsNoTracking()
                .ToListAsync();
            return Ok(suggestions);
        }*/

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchSuggestions(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(); // Trả về danh sách rỗng nếu query rỗng
                }
                // Sử dụng CancellationToken từ HttpContext
                var cancellationToken = HttpContext.RequestAborted;

                // Chuẩn hóa query thành lowercase
                var normalizedQuery = query.ToLower();

                // Thực hiện truy vấn với CancellationToken
                var suggestions = await _context.SanPhams
                    .Where(p => EF.Functions.Like(p.SpName.ToLower(), $"%{normalizedQuery}%")
                                && p.Confirm
                                && !p.IsDeleted)
                    .AsNoTracking()
                    .Select(p => p.SpName)
                    .Distinct() // Loại bỏ các giá trị trùng lặp
                    .ToListAsync(cancellationToken); // Truyền CancellationToken vào truy vấn

                return Ok(suggestions);
            }
            catch (TaskCanceledException)
            {
                // Request bị hủy, trả về trạng thái 499 (Client Closed Request)
                Console.WriteLine("Request bị hủy bởi người dùng!");
                return StatusCode(499, "Request was cancelled by the client.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác (nếu có)
                Console.WriteLine("Lỗi ở server!");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
