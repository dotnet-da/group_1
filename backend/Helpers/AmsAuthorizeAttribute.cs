using AccountsDLL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AmsAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public AccountType AuthorizationLevel { get; set; }

        public AmsAuthorizeAttribute()
        {
            AuthorizationLevel = AccountType.User;
        }
        public AmsAuthorizeAttribute(AccountType level)
        {
            AuthorizationLevel = level;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (Account)context.HttpContext.Items["User"];
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            if (user.Type < AuthorizationLevel)
            {
                // not high enough access rights
                context.Result = new JsonResult(new { message = "Unauthorized, access rights too low" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
        }
    }
}
