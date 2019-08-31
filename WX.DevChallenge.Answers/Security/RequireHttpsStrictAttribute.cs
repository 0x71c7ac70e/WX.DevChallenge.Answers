using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WX.DevChallenge.Answers.Security
{
    /*
     * Used to deny any http requests
     */
    public class RequireHttpsStrictAttribute : RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
        {
            filterContext.Result = new StatusCodeResult((int)HttpStatusCode.HttpVersionNotSupported);
        }
    }
}
