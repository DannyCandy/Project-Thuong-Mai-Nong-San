using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Areas.Admin.Controllers
{
    public class HomeControllers : Controller
    {      
        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
