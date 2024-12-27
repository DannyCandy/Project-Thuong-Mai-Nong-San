using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    /*[ApiController]
    [Route("api/users")]*/
    public class UserController : Controller
    {
        private readonly AgriculturalProductsContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IOrderHistory _orderHistory;
        public UserController(AgriculturalProductsContext context, IUserRepository userRepository, IOrderHistory orderHistory)
        {
            _context = context;
            _userRepository = userRepository;
            _orderHistory = orderHistory;
        }

        [AcceptVerbs("GET","POST")]
        public IActionResult IsEmailExist(string email)
        {
            var IsEmailExist = _context.Users.Any(x => x.Email == email);
            if (IsEmailExist)
            {
                return Json($"Email '{email}' is already in use");
            }
            return Json(true);
        }

        [AcceptVerbs("GET","POST")]
        public IActionResult IsUserNameExist(string userName)
        {
            var IsUserNameExist = _context.Users.Any(x => x.UserName == userName);
            if (IsUserNameExist)
            {
                return Json($"Username '{userName}' is already in use");
            }
            return Json(true);
        }

        [AllowAnonymous, HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous, HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                // code here
                var user = await _userRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    await _userRepository.GenerateForgotPasswordTokenAsync(user);
                    ModelState.Clear();
                    model.EmailSent = true;
                }
                else
                {
                    model.EmailSent = false;
                    ViewBag.ErrMes = "Email không tồn tại trong hệ thống.";
                    return View("EmailErr");
                }
            }
            return View(model);
        }

        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPassword resetPasswordModel = new ResetPassword
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordModel);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _userRepository.ResetPasswordAsync(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory(int month=-1, int year = -1,int pageIndex = 1 )
        {
            string idUser = User.FindFirst("UserId").Value ?? "";
            if (month == -1 && year == -1)
            {
                DateTime current = DateTime.UtcNow;
                month = current.Month;
                year = current.Year;
            }
            if (idUser == "")
            {
                return NotFound("Không tìm thấy thông tin người dùng.");
            }
            var paginatedOrder = await _orderHistory.GetOrderByPaginated(idUser,5, pageIndex, month, year);
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View(paginatedOrder);
        }
    }
}
