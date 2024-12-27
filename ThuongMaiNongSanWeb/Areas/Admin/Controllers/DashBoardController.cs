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
    public class DashBoardController : Controller
    {
        private readonly AgriculturalProductsContext _context;
        public IActionResult Statistics()
        {
            // Gọi API ghi lại lượt truy cập
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");
                client.PostAsync("/api/Statistics/AddVisitor", null);
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }       
    }
}