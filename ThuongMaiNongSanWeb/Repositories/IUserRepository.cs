using Microsoft.AspNetCore.Identity;
using WebApp.Models;

namespace WebApp.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task GenerateForgotPasswordTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ResetPassword model);
        Task<IEnumerable<UserInfo>> GetAllAsync();
    }
}
