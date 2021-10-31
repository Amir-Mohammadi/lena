using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace core.HttpStatusCodeHandler
{
  public class HttpStatusCodeMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IHttpStatusCodeService httpStatusCodeService;
    public HttpStatusCodeMiddleware(RequestDelegate next,
                                    IHttpStatusCodeService httpStatusCodeService)
    {
      this.next = next;
      this.httpStatusCodeService = httpStatusCodeService;
    }
    public async Task Invoke(HttpContext context)
    {
      await next(context);
      await httpStatusCodeService.Apply(context);
    }
  }
}