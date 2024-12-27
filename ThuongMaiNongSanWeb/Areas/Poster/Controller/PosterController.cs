
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApp.Areas.Poster.Models;

namespace WebApp.Areas.Poster.Controllers
{
    [Area("Poster")]
    public class PosterController : Controller
    {
        private readonly AgriculturalProductsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PosterController(AgriculturalProductsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
    }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            // Bao gồm cả Author khi lấy dữ liệu
            var posts = await _context.Posts
                                      .Include(p => p.Images)
                                      .Include(p => p.Author)
                                      .ToListAsync();
            return View(posts);
        }
        // GET: Post/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
                                    .Include(p => p.Images)
                                    .Include(p => p.Author) // Bao gồm thông tin về tác giả
                                    .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [Authorize(Roles = "Poster")]
        // GET: Post/Create
        public IActionResult Create()
        {
            return View(new Post()); // Trả về đối tượng Post mới
        }
        [Authorize(Roles = "Poster")]
        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post model, List<IFormFile> images)
        {
            ModelState.Remove("Author");
            ModelState.Remove("AuthorId");
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    var post = new Post
                    {
                        Title = model.Title,
                        Content = model.Content,
                        AuthorId = user.Id,
                        Author = user,
                        CreatedAt = DateTime.Now,
                        Images = new List<Image>(),
                    };

                    if (images != null && images.Count > 0)
                    {
                        foreach (var image in images)
                        {
                            if (image.Length > 0)
                            {
                                // Lưu hình ảnh và nhận đường dẫn lưu trữ
                                var imageUrl = await SaveImage(image);
                                post.Images.Add(new Image
                                {
                                    ImageUrl = imageUrl
                                });
                            }
                        }
                    }

                    _context.Posts.Add(post);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("PostSuccess");
                }
                catch (Exception)
                {
                    return RedirectToAction("PostError");
                }
            }
            return View();
        }

        // Phương thức để lưu hình ảnh và trả về đường dẫn
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/PostImg", image.FileName);

            // Mở một luồng file để ghi dữ liệu hình ảnh
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                // Sao chép dữ liệu từ luồng hình ảnh tải lên vào luồng file
                await image.CopyToAsync(stream);
            }

            return "/images/PostImg/" + image.FileName;
        }

        [Authorize(Roles = "Poster")]
        public async Task<IActionResult> Edit(int id)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("ForbiddenPageForPost", "Home", new { area = "" });
            }

            string userId = user.Id;

            var post = await _context.Posts
                                     .Include(p => p.Images)
                                     .Include(p => p.Author)
                                     .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null || post.AuthorId != userId) // Kiểm tra quyền sở hữu
            {
                return RedirectToAction("ForbiddenPageForPost", "Home", new {area = ""}); // Chuyển hướng nếu không có quyền
            }

            return View(post);
        }

        [Authorize(Roles = "Poster")]
        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post model, List<IFormFile> newImages)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Author");
            ModelState.Remove("AuthorId");
            ModelState.Remove("Images");
            if (ModelState.IsValid)
            {
                try
                {
                    var post = await _context.Posts.Include(p => p.Images).SingleOrDefaultAsync(m => m.Id == id);
                    if (post == null)
                    {
                        return NotFound();
                    }

                    post.Title = model.Title;
                    post.Content = model.Content;
                    post.UpdatedAt = DateTime.Now; // Cập nhật thời gian 

                    // Handle new images if any
                    if (newImages != null && newImages.Count > 0)
                    {
                        foreach (var image in newImages)
                        {
                            if (image.Length > 0)
                            {
                                // Lưu hình ảnh và nhận đường dẫn lưu trữ
                                var imageUrl = await SaveImage(image);
                                post.Images.Clear();
                                post.Images.Add(new Image { ImageUrl = imageUrl });
                            }
                        }
                    }

                    _context.Posts.Update(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("PostSuccess");
                }
                catch (Exception)
                {
                    return RedirectToAction("PostError");
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Poster")]
        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("ForbiddenPageForPost", "Home", new { area = "" });
            }

            string userId = user.Id;

            var post = await _context.Posts.Include(p => p.Images).Include(p => p.Author).SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.AuthorId != userId) // Kiểm tra quyền sở hữu
            {
                return RedirectToAction("ForbiddenPageForPost", "Home", new { area = "" }); // Chuyển hướng nếu không có quyền
            }
            return View(post);
        }

        // POST: Post/Delete/5
        [Authorize(Roles = "Poster")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.Include(p => p.Images).SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            // Remove images from the server
            if (post.Images != null && post.Images.Count > 0)
            {
                foreach (var image in post.Images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostSuccess");
        }

        [Authorize(Roles = "Poster")]
        // GET: Post/Success
        public IActionResult PostSuccess()
        {
            return View();
        }

        [Authorize(Roles = "Poster")]
        // GET: Post/Error
        public IActionResult PostError()
        {
            return View();
        }

        [Authorize(Roles = "Poster")]
        public IActionResult PostUpdate()
        {
            return View();
        }

        public IActionResult ForbiddenPageForPost()
        {
            return View();
        }

    }
}