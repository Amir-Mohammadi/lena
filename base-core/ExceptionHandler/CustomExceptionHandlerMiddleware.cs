using System;
using System.Threading.Tasks;
using core.RequestLogger;
using Microsoft.AspNetCore.Http;
namespace core.ExceptionHandler
{
  public class CustomExceptionHandlerMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IRequestLoggerService requestLoggerService;
    private readonly IExceptionHandlerService exceptionHandlerService;
    public CustomExceptionHandlerMiddleware(RequestDelegate next,
                                            IRequestLoggerService requestLoggerService,
                                            IExceptionHandlerService exceptionHandlerService)
    {
      this.next = next;
      this.requestLoggerService = requestLoggerService;
      this.exceptionHandlerService = exceptionHandlerService;
    }
    public async Task Invoke(HttpContext httpContext)
    {
      try
      {
        await this.next(httpContext);
      }
      catch (Exception exception)
      {
        await requestLoggerService.Log(context: httpContext, exception: exception);
        await exceptionHandlerService.Handle(httpContext, exception);
      }
    }
  }
}