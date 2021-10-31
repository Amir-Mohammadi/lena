using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
namespace lena.Filters.FilterResult
{
  public class ControllerActionFilter : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
      //TODO fix it sssss
      // (App.Providers.Request as AspRequestInfoProvider).Config(actionContext);
      //#region Add log to database
      // var logEnabled = App.Providers.Storage.ApplicationLogEnabled;
      // if (logEnabled)
      // {
      //   Task.Run(() =>
      //   {
      //     ApplicationManagement applicationManagement = null;
      //     try
      //     {
      //       applicationManagement = App.Api.ApplicationManagment;
      //     }
      //     catch (Exception ex)
      //     {
      //     }
      //     if (applicationManagement != null)
      //     {
      //       var appLog = applicationManagement.AddApplicationLog;
      //       if (actionContext != null && appLog.Success)
      //         actionContext.Request.Headers.Add("RequestLogInfo", new string[] { appLog.Data.Id.ToString(), string.Join(",", appLog.Data.RowVersion) });
      //     }
      //   });
      // }
      //#endregion
      // //Check user has access from External/Internet to ERP App
      // //Check is local request
      // var isLocalRequest = true;
      // var requestIpAddress = NetHelper.GetCurrentHttpContextIPAddress();
      // var getSettingInput = new GetApplicationSettingInput() { Key = SettingKey.LocalIpArddressPaterns.ToString() };
      // var localIpPatterns = App.Api.ApplicationSetting.GetApplicationSetting.Run(getSettingInput);
      // if (localIpPatterns.Data != null)
      // {
      //   var patterns = localIpPatterns.Data.Value.Split(',');
      //   isLocalRequest = patterns.Any(p => requestIpAddress.StartsWith(p));
      // }
      // #region Authorization
      // var noAuthorizeAtt = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>();
      // if (noAuthorizeAtt.Count == 0)
      // {
      //   //برای اجرای این اکشن یوزر حتما باید لاگین کرده باشد و دسترسی لازم را داشته باشد
      //   var permissionInput = new CheckPermissionInput
      //   {
      //     ActionName = actionContext.Request.RequestUri.LocalPath,
      //     ActionParameters = App.Providers.Request.Parameters.ToActionParameter(),
      //     IsExternalRequest = !isLocalRequest
      //   };
      //   var result = App.Api.UserManagement.CheckPermission.Run(permissionInput);
      //   if (!result.Success || result.Data.AccessType == AccessType.Denied)
      //   {
      //     actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
      //     {
      //       Content = new StringContent(Json.Encode(new Result
      //       {
      //         Success = false,
      //         Message = new AcessDeniedException(
      //                 securityActionId: result.Data.SecurityActionId,
      //                 securityActionAddress: result.Data.SecurityActionAddress,
      //                 securityActionName: result.Data.SecurityActionName)
      //             .ToResponse()
      //       }))
      //     };
      //     return;
      //   }
      // }
      // #endregion
      base.OnActionExecuting(actionContext);
    }
    public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
    {
      //TODO fix it sssss
      // base.OnActionExecuted(actionExecutedContext);
      // Task.Run(() =>
      // {
      //   IEnumerable<string> requestLogInfo;
      //   actionExecutedContext.Request.Headers.TryGetValues("RequestLogInfo", out requestLogInfo);
      //   if (requestLogInfo != null && requestLogInfo.Count() != 0)
      //   {
      //     var infoArray = requestLogInfo.ToArray();
      //     var input = new EditApplicationLogInput();
      //     input.Id = Convert.ToInt32(infoArray[0]);
      //     input.RowVersion = infoArray[1].Split(',').Select(x => Convert.ToByte(x)).ToArray();
      //     input.RequestEndTime = DateTime.UtcNow;
      //     App.Api.ApplicationManagment.EditApplicationLog.Run(input);
      //   }
      // });
      // try
      // {
      //   if (!App.Providers.Request.IsFailed)
      //   {
      //     App.Providers.PersistentLogger.Response = App.Providers.Request.Response;
      //     App.Providers.PersistentLogger.Finish();
      //   }
      // }
      // catch (Exception e)
      // {
      // }
    }
  }
}