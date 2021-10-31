using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace core.Security.ApiProtector
{
  public class ApiProtectorMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IApiProtectorService apiProtectorService;

    public ApiProtectorMiddleware(RequestDelegate next,
                                  IApiProtectorService apiProtectorService)
    {
      this.next = next;
      this.apiProtectorService = apiProtectorService;
    }
    public async Task Invoke(HttpContext context)
    {
      await apiProtectorService.Protect(context);
      await next(context);
    }
  }
}