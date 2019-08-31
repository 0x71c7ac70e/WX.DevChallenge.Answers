using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WX.DevChallenge.Answers.Security
{
    /*
     * Used to deny any http requests
     */
    public class RequireHttpsStrictAttribute : RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
        {
            filterContext.Result = new StatusCodeResult(StatusCodes.Status505HttpVersionNotsupported);
        }
    }
}
