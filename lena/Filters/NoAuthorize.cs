using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
namespace lena.Filters
{
  public class NoAuthorize : AuthorizeAttribute
  {
    // public override void OnAuthorization(AuthorizationContext filterContext)
    // {
    //   //check for role in session variable "ADMIN_ROLE"
    //   //if not valid user then set
    //   //filterContext.Result = new RedirectResult(URL)
    // }
  }
}