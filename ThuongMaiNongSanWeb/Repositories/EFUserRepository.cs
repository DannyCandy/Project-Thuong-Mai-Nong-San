using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.Common;
using System;
using System.Net.Http.Headers;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public EFUserRepository(UserManager<ApplicationUser> userManager, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task GenerateForgotPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if(!string.IsNullOrEmpty(token))
            {
                /*await SendForgotPasswordEmail(user, token);*/
                await SendForgotPasswordEmailApi(user,token);
            }
        }

        //Thêm mới cách dùng api gửi mail
        private async Task SendForgotPasswordEmailApi(ApplicationUser user, String token) {
            string appDomain = _configuration.GetValue<string>("Application:AppDomain");
            string confirmationLink = _configuration.GetValue<string>("Application:ForgotPassword");
            string url = _configuration.GetValue<string>("SMTPConfig:Url");
            string apiToken = _configuration.GetValue<string>("SMTPConfig:ApiToken");
            string senderMail = _configuration.GetValue<string>("SMTPConfig:SenderAddress");
            string senderName = _configuration.GetValue<string>("SMTPConfig:SenderDisplayName");
            string resetPwdUrl = string.Format(appDomain + confirmationLink, user.Id, token);
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
                    "\"email\": \"" + user.Email + "\",\n      " +
                    "\"name\": \"" + user.TenKhachHang + "\"\n    }\n  ],\n  " +
                    "\"from\": {\n    " +
                    "\"email\": \""+senderMail+"\",\n    " +
                    "\"name\": \""+senderName+"\"\n  },\n  " +
                    "\"subject\": \"Lấy lại mật khẩu\",\n  " +
                    "\"text\": \"Hãy lấy lại mật khẩu bằng cách truy cập đường link sau: " + resetPwdUrl + "\",\n  " +
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

        private async Task SendForgotPasswordEmail(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetValue<string>("Application:AppDomain");
            string confirmationLink = _configuration.GetValue<string>("Application:ForgotPassword");
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                    new KeyValuePair<string, string>("{{Link}}",
                    string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };
            await _emailService.SendEmailForForgotPassword(options);
        }

        //Xử lý lấy lại password
        public async Task<IdentityResult> ResetPasswordAsync(ResetPassword model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }

        public async Task<IEnumerable<UserInfo>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userWithRolesList = new List<UserInfo>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userWithRoles = new UserInfo
                {
                    Id = user.Id,
                    Username = user.UserName,
                    TenKhachHang = user.TenKhachHang,
                    DiaChi = user.DiaChi,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Avatar = user.Avatar,
                    Role = roles[0],
                };
                userWithRolesList.Add(userWithRoles);
            }

            return userWithRolesList;
        }
    }
}
