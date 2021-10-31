using System;
using Microsoft.AspNetCore.Mvc.Filters;
namespace core.Security.ApiProtector
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public class ApiProtectorAttribute : ActionFilterAttribute
  {
    public ApiProtectionType ApiProtectionType { get; set; }
    public int Limit { get; set; }
    public int TimeWindowSecond { get; set; }
    public int? AppRoles { get; set; }
    public int PenaltySecond { get; set; }
    public ApiProtectorAttribute(ApiProtectionType ApiProtectionType, int Limit, int TimeWindowSecond, int PenaltySecond = 0)
    : this(ApiProtectionType, Limit, TimeWindowSecond, PenaltySecond, null)
    {
    }
    private ApiProtectorAttribute(ApiProtectionType ApiProtectionType, int Limit, int TimeWindowSecond, int PenaltySecond = 0, int? AppRoles = null)
    {
      this.ApiProtectionType = ApiProtectionType;
      this.Limit = Limit;
      this.TimeWindowSecond = TimeWindowSecond;
      this.PenaltySecond = PenaltySecond;
      this.AppRoles = AppRoles;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      base.OnActionExecuting(context);
    }
  }
}