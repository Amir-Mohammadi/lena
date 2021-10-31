using lena.Models.Application.EntityLog;
using lena.Services.Core;
using Microsoft.AspNetCore.Mvc.Filters;
namespace lena.Filters
{
  public class EntityLog : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
      try
      {
        var input = new AddEntityLogInput
        {
          ApiParams = Newtonsoft.Json.JsonConvert.SerializeObject(actionContext.ActionArguments),
          //TODO fix it sssss
          // Api = actionContext.Request.RequestUri.AbsoluteUri
        };
        //TODO fix it sssss
        // App.Api.ApplicationManagment.AddEntityLog.Run(input: input);
      }
      catch { }
    }
  }
}