using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.Data;
using WebApp.MobileApp.DTO;
using WebApp.MobileApp.Repositories.Abstract;
using WebApp.Models;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace WebApp.MobileApp.Controllers
{
    [Route("api/app/[controller]/{action}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AgriculturalProductsContext _ctx;
        private readonly ITokenService _service;
        private readonly UserManager<ApplicationUser> userManager;
        public UserController(AgriculturalProductsContext ctx, ITokenService service, UserManager<ApplicationUser> userManager)
        {
            this._ctx = ctx;
            this._service = service;
            this.userManager = userManager;
        }

        //Thao tác khi đã login
        [HttpPost]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest tokenApiModel) 
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request: TokenApiModel not found!");
            try
            {
                string? accessToken = tokenApiModel.AccessToken;
                string? refreshToken = tokenApiModel.RefreshToken;
                var principal = _service.GetPrincipalFromExpiredToken(accessToken!);
                /*var username = principal.Identity?.Name;
                var user = _ctx.TokenInfos.SingleOrDefault(u => u.UserId == username);*/

                //Sử dụng userId thay vì username
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = _ctx.TokenInfos.SingleOrDefault(u => u.UserId == userId);
                if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now)
                    return BadRequest("Invalid client request: Some unexpected issues founded on user information.");
                var newAccessToken = _service.GetToken(principal.Claims);
                var newRefreshToken = _service.GetRefreshToken();
                user.RefreshToken = newRefreshToken;
                _ctx.SaveChanges();
                return Ok(new RefreshTokenRequest()
                {
                    AccessToken = newAccessToken.TokenString,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Some errors occur in validating user's claims: " + ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult Logout()
        {
            try
            {
                var username = User.Identity.Name;
                var user = _ctx.TokenInfos.SingleOrDefault(u => u.Usename == username);
                if (user is null)
                    return BadRequest("Can't specify username from user.");
                user.RefreshToken = null;
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var status = new Status();
            // check validations
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass all the valid fields\nOr new password is not match.";
                return BadRequest(status);
            }
            // lets find the user
            ApplicationUser? user;
            /*if (model.UsernameOrEmail?.IndexOf('@') > -1)
            {
                user = await userManager.FindByEmailAsync(model.UsernameOrEmail);
            }
            else
            {
                user = await userManager.FindByNameAsync(model.UsernameOrEmail);
            }*/
            user = await userManager.FindByIdAsync(model.UserId);
            if (user is null)
            {
                status.StatusCode = 0;
                status.Message = "Không tìm thấy user.";
                return BadRequest(status);
            }
            // check current password
            if (!await userManager.CheckPasswordAsync(user, model.CurrentPassword))
            {
                status.StatusCode = 0;
                status.Message = "Invalid current password";
                return BadRequest(status);
            }

            // change password here
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Failed to change password";
                return BadRequest(status);
            }
            status.StatusCode = 1;
            status.Message = "Password has changed successfully";
            return Ok(status);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> UpdateAvatar([FromForm] UpdateAvatarModel model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass all the required fields";
                return BadRequest(status);
            }

            var user = await userManager.FindByIdAsync(model.IdUser);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Can't find user.";
                return NotFound(status);
            }
            try
            {
                //Create user avatar
                if (model.Avatar.Length > 0)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await model.Avatar.CopyToAsync(dataStream);
                        user.Avatar = dataStream.ToArray();
                    }
                }
                _ctx.Update(user);
                await _ctx.SaveChangesAsync();
                var imageData = Convert.ToBase64String(user.Avatar);
                status.StatusCode = 1;
                status.Message = "Thay đổi ảnh đại diện thành công";
                return Ok(new
                {
                    status.Message,
                    status.StatusCode,
                    UserAvatar = imageData,
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Can not update your avatar because: " + ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfo model)
        {
            var status = new Status();
            try
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    status.StatusCode = 0;
                    status.Message = "Không tìm thấy người dùng";
                    return NotFound(status);
                }
                var IsUserNameExist = _ctx.Users.Any(x => x.UserName == model.UserName);
                if (IsUserNameExist)
                {
                    status.StatusCode = 0;
                    status.Message = "Tên người dùng đã tồn tại.";
                    return NotFound(status);
                }
                user.UserName = model.UserName ?? user.UserName;
                user.TenKhachHang = model.TenKhachHang ?? user.TenKhachHang;
                user.PhoneNumber = model.Phone ?? user.PhoneNumber;
                user.DiaChi = model.DiaChi ?? user.DiaChi;
                _ctx.Update(user);
                await _ctx.SaveChangesAsync();

                status.StatusCode = 1;
                status.Message = "Cập nhật thông tin thành công.";

                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest("Can not update user because: " + ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailModel model)
        {
            var status = new Status();
            try
            {
                // check validations
                if (!ModelState.IsValid)
                {
                    status.StatusCode = 0;
                    status.Message = "Please pass all the valid fields";
                    return BadRequest(status);
                }
                // lets find the user
                ApplicationUser? user;
                user = await userManager.FindByIdAsync(model.userId);
                if (user is null)
                {
                    status.StatusCode = 0;
                    status.Message = "Không tìm thấy người dùng.";
                    return BadRequest(status);
                }
                if (user.Email == model.Email)
                {
                    status.StatusCode = 1;
                    status.Message = "Đã cập nhật thông tin người dùng.";
                    return Ok(status);
                }
                var IsEmailExist = _ctx.Users.Any(x => x.Email == model.Email);
                if(IsEmailExist)
                {
                    status.StatusCode = 0;
                    status.Message = "Email đã được đặt, vui lòng chọn email khác.";
                    return BadRequest(status);
                }
                // change email process
                
                var emailConfirmCode = await userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                var confirmResult = await userManager.ChangeEmailAsync(user, model.Email,emailConfirmCode);

                if (confirmResult.Succeeded)
                {
                    status.StatusCode = 1;
                    status.Message = "Đã cập nhật thông tin người dùng.";
                    return Ok(status);
                }
                status.StatusCode = 0;
                status.Message = "Có lỗi xảy ra khi cập nhật email.";
                return BadRequest(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }


        }

        //Xử lý lấy thông tin hiển thị

        //Trang chính profile chứa các option
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var status = new Status();
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    status.StatusCode = 0;
                    status.Message = "Không tìm thấy người dùng";
                    return NotFound(status);
                }
                var imageData = Convert.ToBase64String(user.Avatar);
                status.StatusCode = 1;
                status.Message = "Lấy thông tin người dùng thành công.";

                return Ok(new
                {
                    TenKhachHang = user.TenKhachHang!,
                    Email = user.Email!,
                    Avatar = imageData,
                    Phone = user.PhoneNumber ?? "",
                    user.DiaChi,
                    user.UserName,
                    StatusCode = status.StatusCode!,
                    Message = status.Message!
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Không thể lấy thông tin người dùng bởi vì: " + ex.Message);

            }
        }

    }
}
