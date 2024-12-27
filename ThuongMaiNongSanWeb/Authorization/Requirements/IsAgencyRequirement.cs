using Microsoft.AspNetCore.Authorization;

namespace WebApp.Authorization.Requirements
{
    public class IsAgencyRequirement: IAuthorizationRequirement
    {
        public IsAgencyRequirement()
        {
        }
    }
}
