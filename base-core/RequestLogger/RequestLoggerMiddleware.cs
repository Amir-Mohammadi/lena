using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace core.RequestLogger
{
  public class RequestLoggerMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IRequestLoggerService requestLoggerService;
    public RequestLoggerMiddleware(RequestDelegate next,
                                   IRequestLoggerService requestLoggerService)
    {
      this.next = next;
      this.requestLoggerService = requestLoggerService;
    }
    public async Task Invoke(HttpContext context)
    {
      await next(context);
      await requestLoggerService.Log(context);
    }
  }
}