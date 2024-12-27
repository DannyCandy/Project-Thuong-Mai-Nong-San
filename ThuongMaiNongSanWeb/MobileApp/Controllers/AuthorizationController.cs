using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApp.Data;
using WebApp.MobileApp.DTO;
using WebApp.MobileApp.Models;
using WebApp.MobileApp.Repositories.Abstract;
using Microsoft.Extensions.Options;
using WebApp.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using WebApp.Repositories;
using NuGet.Common;

namespace WebApp.MobileApp.Controllers
{
    [Route("api/app/[controller]/{action}")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly AgriculturalProductsContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SMTPConfig _smtpConfig;
        private readonly ITokenService _tokenService;
        public AuthorizationController(AgriculturalProductsContext context, IOptions<SMTPConfig> smtpConfig,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService
            )
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _tokenService = tokenService;
            _smtpConfig = smtpConfig.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new LoginResponse
                {
                    StatusCode = 0,
                    Message = "Please pass all the required fields.",
                    Token = "",
                    Expiration = null
                });
            }
            ApplicationUser? user = null;
            if (model.UserNameOrEmail?.IndexOf('@') > -1)
            {
                user = await userManager.FindByEmailAsync(model.UserNameOrEmail);
            }
            else
            {
                user = await userManager.FindByNameAsync(model.UserNameOrEmail);
            }
            if (user == null)
            {
                return NotFound(
                new LoginResponse
                {
                    StatusCode = 0,
                    Message = "Invalid Username or Email",
                    Token = "",
                    Expiration = null
                });
            }
                
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var imageData = Convert.ToBase64String(user.Avatar);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = _tokenService.GetToken(authClaims);
                var refreshToken = _tokenService.GetRefreshToken();
                var tokenInfo = _context.TokenInfos.FirstOrDefault(a => a.Usename == user.UserName);
                if (tokenInfo == null)
                {
                    var info = new TokenInfo
                    {
                        UserId = user.Id,
                        Usename = user.UserName,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiry = DateTime.Now.AddDays(3)
                    };
                    _context.TokenInfos.Add(info);
                }
                else
                {
                    if(tokenInfo.VerifyOtp is not null)
                    {
                        return BadRequest(
                        new LoginResponse
                        {
                            StatusCode = 0,
                            Message = "Người dùng chưa xác thực đăng kí.",
                            Token = "",
                            Expiration = null
                        });
                    }
                    tokenInfo.RefreshToken = refreshToken;
                    tokenInfo.RefreshTokenExpiry = DateTime.Now.AddDays(3);
                }
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(
                        new LoginResponse
                        {
                            StatusCode = 0,
                            Message = "Xảy ra lỗi trong quá trình đăng nhập.",
                            Token = "",
                            Expiration = null
                        });
                }
                return Ok(new LoginResponse
                {
                    UserId = user.Id,
                    Name = user.TenKhachHang,
                    Username = user.UserName,
                    UserAvatar = imageData,
                    Email = user.Email,
                    Token = token.TokenString,
                    RefreshToken = refreshToken,
                    Diachi = user.DiaChi,
                    Phone = user.PhoneNumber,
                    Expiration = token.ValidTo,
                    StatusCode = 1,
                    Message = "Logged in"
                });

            }
            //login failed condition
            return BadRequest(
                new LoginResponse
                {
                    StatusCode = 0,
                    Message = "Invalid Username or Password",
                    Token = "",
                    Expiration = null
                });
        }

        //Hàm xử lý chuyên biệt của class
        private static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        private static string HashOtp(string otp)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        //Quá trình đăng kí
        private async Task SendConfirmationEmail(string email, string tenKh)
        {
            try
            {
                /*//Khởi tạo mail
                var bodyBuilder = new StringBuilder();
                bodyBuilder.Append("<h2>Xác minh OTP thành công!</h2>");
                bodyBuilder.Append("<p>Cảm ơn bạn đã hoàn tất đăng ký tài khoản.<br />Chúc bạn có những trải nghiệm mua sắm tuyệt vời tại eGrocery</p>");
                bodyBuilder.Append("<p>Xin cảm ơn!</p>");
                MimeMessage mailMessage = new MimeMessage
                {
                    Subject = "Xin chào "+tenKh+", hãy xác nhận đăng kí.",
                };
                mailMessage.From.Add(new MailboxAddress(_smtpConfig.SenderDisplayName, _smtpConfig.SenderAddress));
                mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = bodyBuilder.ToString()
                };
                mailMessage.To.Add(new MailboxAddress("Customer", email));
                //Tạo port
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Connect(_smtpConfig.Host, _smtpConfig.Port, true);
                smtpClient.Authenticate(_smtpConfig.UserName, _smtpConfig.Password);
                await smtpClient.SendAsync(mailMessage);*/

                //Gọi mail api
                string url = _smtpConfig.Url!;
                string apiToken = _smtpConfig.ApiToken!;
                string senderMail =_smtpConfig.SenderAddress!;
                string senderName = _smtpConfig.SenderDisplayName!;
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Headers =
                {
                    { "Accept", "application/json" },
                    { "Api-Token", apiToken },
                },
                    Content = new StringContent(
                        "{\n    \"to\": [\n    {\n      " +
                        "\"email\": \"" + email + "\",\n      " +
                        "\"name\": \"" + tenKh + "\"\n    }\n  ],\n  " +
                        "\"from\": {\n    " +
                        "\"email\": \"" + senderMail + "\",\n    " +
                        "\"name\": \"" + senderName + "\"\n  },\n  " +
                        "\"subject\": \"XÁC MINH OTP THÀNH CÔNG\",\n  " +
                        "\"text\": \"Cảm ơn bạn đã hoàn tất đăng ký tài khoản. Chúc bạn có những trải nghiệm mua sắm tuyệt vời tại eGrocery\",\n  " +
                        "\"category\": \"API Test\"\n}"
                    )
                    {
                        Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    /*var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);*/
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể khởi tạo mail vì: " + ex.Message);
            }
        }

        private async Task SendRegisterOtpEmail(string email,string tenKH, string otp)
        {
            /*try
            {
                //Khởi tạo mail
                var bodyBuilder = new StringBuilder();
                bodyBuilder.Append("<h2>Xác minh OTP</h2>");
                bodyBuilder.Append("<p>Cảm ơn bạn đã đăng ký tài khoản. Dưới đây là mã OTP của bạn:</p>");
                bodyBuilder.Append($"<p><strong>Mã OTP:</strong> {otp}</p>");
                bodyBuilder.Append("<p>Vui lòng nhập mã OTP này vào ứng dụng eGrocery để hoàn tất quá trình đăng ký.</p>");
                bodyBuilder.Append("<p>Xin cảm ơn!</p>");
                MimeMessage mailMessage = new MimeMessage
                {
                    Subject = "Xin chào " + tenKH + ", hãy xác nhận đăng kí.",
                };
                mailMessage.From.Add(new MailboxAddress(_smtpConfig.SenderDisplayName, _smtpConfig.SenderAddress));
                mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = bodyBuilder.ToString()
                };
                mailMessage.To.Add(new MailboxAddress("Customer", email));
                //Tạo port
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Connect(_smtpConfig.Host, _smtpConfig.Port, true);
                smtpClient.Authenticate(_smtpConfig.UserName, _smtpConfig.Password);
                await smtpClient.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể khởi tạo mail vì: "+ex.Message);
            }*/

            //Gọi mail api
            string url = _smtpConfig.Url!;
            string apiToken = _smtpConfig.ApiToken!;
            string senderMail = _smtpConfig.SenderAddress!;
            string senderName = _smtpConfig.SenderDisplayName!;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Api-Token", apiToken },
                },
                Content = new StringContent(
                    "{\n    \"to\": [\n    {\n      " +
                    "\"email\": \"" + email + "\",\n      " +
                    "\"name\": \"" + tenKH + "\"\n    }\n  ],\n  " +
                    "\"from\": {\n    " +
                    "\"email\": \"" + senderMail + "\",\n    " +
                    "\"name\": \"" + senderName + "\"\n  },\n  " +
                    "\"subject\": \"XÁC MINH OTP\",\n  " +
                    "\"text\": \"Cảm ơn bạn đã đăng ký tài khoản. Đây là mã OTP đăng kí của bạn: "+otp+"\",\n  " +
                    "\"category\": \"API Test\"\n}"
                )
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                /*var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);*/
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResendOTP([FromBody] ResendOtp model)
        {
            var status = new Status();
            try
            {
                if (!ModelState.IsValid)
                {
                    status.StatusCode = 0;
                    status.Message = "Request không đúng định dạng.";
                    return BadRequest(status);
                }
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    status.StatusCode = 0;
                    status.Message = "Email người dùng không tồn tại.";
                    return NotFound(status);
                }
                var userToken = await _context.TokenInfos.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (userToken == null)
                {
                    status.StatusCode = 0;
                    status.Message = "Không có thông tin về Otp đăng kí của người dùng.";
                    return NotFound(status);
                }
                var otp = GenerateOtp();
                userToken.VerifyOtp = HashOtp(otp); // Hashing the OTP before storing
                userToken.VerifyTokenExpiry = DateTime.UtcNow.AddMinutes(5); // OTP valid for 5 minutes
                await _context.SaveChangesAsync();
                await SendRegisterOtpEmail(user.Email, user.TenKhachHang, otp);
                status.StatusCode = 1;
                status.Message = "Đã gửi lại Otp đăng kí thành công";
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest("Xảy ra lỗi trong quá trình gửi lại Otp đăng kí: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtp model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Request không đúng định dạng.";
                return BadRequest(status);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Email người dùng không tồn tại.";
                return NotFound(status);
            }
            var userToken = await _context.TokenInfos.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (userToken == null)
            {
                status.StatusCode = 0;
                status.Message = "Otp chưa được tạo ra cho người dùng.";
                return NotFound(status);
            }

            try
            {
                if (userToken.VerifyTokenExpiry < DateTime.UtcNow)
                {
                    status.StatusCode = 0;
                    status.Message = "Otp hết hạn.";
                    return BadRequest(status);
                }

                var hashedOtp = HashOtp(model.Otp);
                if (userToken.VerifyOtp == hashedOtp)
                {
                    userToken.EmailConfirmed = true;
                    userToken.ActiveDate = DateTime.Now; // Gán ActiveDate sau khi xác nhận OTP
                    userToken.VerifyOtp = null; // Clear OTP after successful verification
                    userToken.VerifyTokenExpiry = null;
                    await SendConfirmationEmail(user.Email, user.TenKhachHang);
                    await _context.SaveChangesAsync();

                    status.StatusCode = 1;
                    status.Message = "Xác minh Otp thành công.\nHãy tiến hành đăng nhập lại.";
                    return Ok(status);
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Otp không hợp lệ";
                    return BadRequest(status);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Tiếp cận không hợp lệ: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registration([FromForm] RegisterModel model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass all the required fields";
                return BadRequest(status);
            }
            // check if user exists
            var IsUserNameExist = _context.Users.Any(x => x.UserName == model.Username);
            if (IsUserNameExist)
            {
                status.StatusCode = 0;
                status.Message = "Username is existed";
                return NotFound(status);
            }
            var existingEmailUser = await userManager.FindByEmailAsync(model.Email);
            if (existingEmailUser != null)
            {
                status.StatusCode = 0;
                status.Message = "Email is existed";
                return NotFound(status);
            }
            try
            {
                var user = Activator.CreateInstance<ApplicationUser>();

                user.UserName = model.Username;
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.Email = model.Email;
                user.DiaChi = model.DiaChi;
                user.TenKhachHang = model.TenKhachHang;


                //Create user avatar
                if (model.Avatar.Length > 0)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await model.Avatar.CopyToAsync(dataStream);
                        user.Avatar = dataStream.ToArray();
                    }
                }

                // create a user here
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    status.StatusCode = 0;
                    status.Message = "Username không hợp lệ.";
                    return BadRequest(status);
                }
                // add roles here
                if (!await roleManager.RoleExistsAsync(model.Role))
                {
                    await roleManager.CreateAsync(new IdentityRole(model.Role));
                }
                if (await roleManager.RoleExistsAsync(model.Role))
                {
                    await userManager.AddToRoleAsync(user, model.Role);
                }
                //Tạo Otp token
                var otp = GenerateOtp();
                var info = new TokenInfo
                {
                    UserId = user.Id,
                    Usename = user.UserName,
                    VerifyOtp = HashOtp(otp),
                    VerifyTokenExpiry = DateTime.UtcNow.AddMinutes(5), // OTP valid for 5 minutes
                };
                _context.TokenInfos.Add(info);
                await SendRegisterOtpEmail(user.Email, user.TenKhachHang, otp);
                await _context.SaveChangesAsync();


                status.StatusCode = 1;
                status.Message = "Hãy xác thực email đăng kí bằng Otp mà chúng tôi đã gửi qua email đăng kí.";
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest("Can not create user because: " + ex.Message);
            }
        }

        //Quá trình lấy lại mật khẩu
        private async Task SendResetPasswordOtpEmail(string email, string tenKH, string otp)
        {
            /*try
            {
                //Khởi tạo mail
                var bodyBuilder = new StringBuilder();
                bodyBuilder.Append("<h2>Xác minh OTP Lấy lại mật khẩu</h2>");
                bodyBuilder.Append("<p>Để lấy lại mật khẩu của mình hãy dùng mã Otp sau. Dưới đây là mã OTP của bạn:</p>");
                bodyBuilder.Append($"<p><strong>Mã OTP:</strong> {otp}</p>");
                bodyBuilder.Append("<p>Vui lòng nhập mã OTP này vào ứng dụng eGrocery để hoàn tất quá trình lấy lại mật khẩu.</p>");
                bodyBuilder.Append("<p>Xin cảm ơn!</p>");
                MimeMessage mailMessage = new MimeMessage
                {
                    Subject = "Xin chào " + tenKH + ", hãy xác nhận lấy lại mật khẩu.",
                };
                mailMessage.From.Add(new MailboxAddress(_smtpConfig.SenderDisplayName, _smtpConfig.SenderAddress));
                mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = bodyBuilder.ToString()
                };
                mailMessage.To.Add(new MailboxAddress("Customer", email));
                //Tạo port
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Connect(_smtpConfig.Host, _smtpConfig.Port, true);
                smtpClient.Authenticate(_smtpConfig.UserName, _smtpConfig.Password);
                await smtpClient.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể khởi tạo mail vì: " + ex.Message);
            }*/

            //Gọi api gửi mail
            string url = _smtpConfig.Url!;
            string apiToken = _smtpConfig.ApiToken!;
            string senderMail = _smtpConfig.SenderAddress!;
            string senderName = _smtpConfig.SenderDisplayName!;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Api-Token", apiToken },
                },
                Content = new StringContent(
                    "{\n    \"to\": [\n    {\n      " +
                    "\"email\": \"" + email + "\",\n      " +
                    "\"name\": \"" + tenKH + "\"\n    }\n  ],\n  " +
                    "\"from\": {\n    " +
                    "\"email\": \"" + senderMail + "\",\n    " +
                    "\"name\": \"" + senderName + "\"\n  },\n  " +
                    "\"subject\": \"XÁC NHẬN LẤY LẠI MẬT KHẨU\",\n  " +
                    "\"text\": \"Để lấy lại mật khẩu của mình hãy dùng mã Otp sau: " + otp + "\",\n  " +
                    "\"category\": \"API Test\"\n}"
                )
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                /*var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);*/
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendOTPResetPassword([FromBody] ResendOtp model)
        {
            var status = new Status();
            try
            {
                if (!ModelState.IsValid)
                {
                    status.StatusCode = 0;
                    status.Message = "Request không đúng định dạng.";
                    return BadRequest(status);
                }
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    status.StatusCode = 0;
                    status.Message = "Email người dùng không tồn tại.";
                    return NotFound(status);
                }
                var userToken = await _context.TokenInfos.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (userToken == null)
                {
                    status.StatusCode = 0;
                    status.Message = "Không có thông tin về Otp resetpassword của người dùng.";
                    return NotFound(status);
                }
                var otp = GenerateOtp();
                userToken.ResetPasswordOtp = HashOtp(otp); // Hashing the OTP before storing
                userToken.ResetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);
                userToken.ResetPasswordTokenExpiry = DateTime.UtcNow.AddDays(1); // OTP valid for 1 days
                await _context.SaveChangesAsync();
                await SendResetPasswordOtpEmail(user.Email, user.TenKhachHang, otp);
                status.StatusCode = 1;
                status.Message = "Đã gửi lại Otp resetpassword thành công";
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest("Xảy ra lỗi trong quá trình gửi lại Otp resetpassword: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordApp model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Request không đúng định dạng.\nHoặc mật khẩu không khớp nhau.";
                return BadRequest(status);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Email người dùng không tồn tại.";
                return NotFound(status);
            }
            var userToken = await _context.TokenInfos.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (userToken == null)
            {
                status.StatusCode = 0;
                status.Message = "Otp chưa được tạo ra cho người dùng.";
                return NotFound(status);
            }

            try
            {
                if (userToken.ResetPasswordTokenExpiry < DateTime.UtcNow)
                {
                    status.StatusCode = 0;
                    status.Message = "Otp hết hạn.";
                    return BadRequest(status);
                }

                var hashedOtp = HashOtp(model.Otp!);
                if (userToken.ResetPasswordOtp == hashedOtp)
                {
                    await userManager.ResetPasswordAsync(user, userToken.ResetPasswordToken, model.ConfirmNewPassword);

                    userToken.ResetPasswordOtp = null; // Clear OTP after successful verification
                    userToken.ResetPasswordTokenExpiry = null;
                    userToken.ResetPasswordToken = null;

                    await _context.SaveChangesAsync();

                    status.StatusCode = 1;
                    status.Message = "Cập nhật mật khẩu thành công.\nHãy đăng nhập lại";
                    return Ok(status);
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Otp không hợp lệ";
                    return BadRequest(status);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Tiếp cận không hợp lệ: " + ex.Message);
            }
        }
    }
}
