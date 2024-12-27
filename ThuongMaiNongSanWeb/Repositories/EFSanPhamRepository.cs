using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class EFSanPhamRepository : ISanPhamRepository
    {
        private readonly AgriculturalProductsContext _context;

        public EFSanPhamRepository(AgriculturalProductsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _context.SanPhams.Where(c => c.IsDeleted == false).ToListAsync();
        }

        public async Task<SanPham> GetByIdAsync(string id)
        {
            return await _context.SanPhams.FindAsync(id);
        }

        public async Task AddAsync(SanPham product)
        {
            _context.SanPhams.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SanPham product)
        {
            _context.SanPhams.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var product = await _context.SanPhams.FindAsync(id);
            _context.SanPhams.Remove(product);
            await _context.SaveChangesAsync();
        }


        public async Task<PaginatedList<SanPham>> GetProductByPaginated(int pageSize, int pageIndex, string category = "", string orderBy = "", string searchName = "")
        {
            if(searchName is null)
            {
                searchName = string.Empty;
            }
            if(searchName == "")
            {
                if (category == "")
                {
                    IQueryable<SanPham> productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IsDeleted == false);
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return paginatedProducts;
                }
                else
                {
                    IQueryable<SanPham> productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IsDeleted == false) 
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IsDeleted == false);
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return paginatedProducts;
                }
            }
            else
            {
                searchName = searchName.ToLower();
                if (category == "")
                {
                    IQueryable<SanPham> productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false);
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return paginatedProducts;
                }
                else
                {
                    IQueryable<SanPham> productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IsDeleted == false);
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return paginatedProducts;
                }
            }
        }

        public async Task<PaginatedList<SanPham>> GetProductByPaginatedOfDaiLy(string idDaiLy,int pageSize, int pageIndex, string category = "", string orderBy = "", string searchName = "")
        {
            if (searchName is null)
            {
                searchName = string.Empty;
            }
            if (searchName == "")
            {
                if (category == "")
                {
                    IQueryable<SanPham> productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false);
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return paginatedProducts;
                }
                else
                {
                    IQueryable<SanPham> productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false);
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return paginatedProducts;
                }
            }
            else
            {
                searchName = searchName.ToLower();
                if (category == "")
                {
                    IQueryable<SanPham> productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.SpName.ToLower().Contains(searchName) && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false);
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return paginatedProducts;
                }
                else
                {
                    IQueryable<SanPham> productsQuery = null;
                    if (orderBy == "asc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                            .OrderBy(p => p.Price); // Sắp xếp sản phẩm theo giá tăng dần
                    }
                    else if (orderBy == "desc")
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false)
                            .OrderByDescending(p => p.Price); // Sắp xếp sản phẩm theo giá giảm dần
                    }
                    else
                    {
                        productsQuery = _context.SanPhams
                            .Include(p => p.IdcategoryNavigation)
                            .Where(p => p.Confirm == true && p.Idcategory == category && p.SpName.ToLower().Contains(searchName) && p.IddaiLyNavigation.IdDaiLy == idDaiLy && p.IsDeleted == false);
                    }
                    var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(productsQuery,
                    pageIndex, pageSize);
                    return paginatedProducts;
                }
            }

        }
    }

}
