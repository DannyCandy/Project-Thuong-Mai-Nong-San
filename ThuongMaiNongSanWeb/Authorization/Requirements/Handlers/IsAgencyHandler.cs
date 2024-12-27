using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Authorization.Requirements.Handlers
{
    public class IsAgencyHandler : AuthorizationHandler<IsAgencyRequirement>
    {
        private readonly AgriculturalProductsContext _context;
        public IsAgencyHandler(AgriculturalProductsContext context)
        {
            _context = context;
        }
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, IsAgencyRequirement requirement)
        {
            if (context.User.FindFirst("UserId") is null)
            {
                // Redirect hoặc hiển thị trang đăng nhập
                context.Fail();
                return Task.CompletedTask;
            }
            var userId = context.User.FindFirst("UserId")?.Value ?? "";
            var isConfirmed = await _context.DaiLyOnlines.FirstOrDefaultAsync(dl => dl.IdUser == userId && dl.Confirm);
            if(isConfirmed is null) 
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
