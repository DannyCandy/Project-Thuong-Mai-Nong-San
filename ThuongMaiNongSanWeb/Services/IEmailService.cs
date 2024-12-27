using WebApp.Models;

namespace WebApp.Services
{
    public interface IEmailService
    {
        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}
