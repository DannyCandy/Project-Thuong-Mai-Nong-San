using System.Security.Claims;
using WebApp.MobileApp.DTO;

namespace WebApp.MobileApp.Repositories.Abstract
{
    public interface ITokenService
    {
        TokenResponse GetToken(IEnumerable<Claim> claim);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
