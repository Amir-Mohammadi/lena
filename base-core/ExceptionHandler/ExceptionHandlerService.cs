using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
namespace core.ExceptionHandler
{
  public class ExceptionHandlerService : IExceptionHandlerService
  {
    private readonly IHostEnvironment hostEnvironment;
    public ExceptionHandlerService(IHostEnvironment hostEnvironment)
    {
      this.hostEnvironment = hostEnvironment;
    }
    public async Task Handle(HttpContext httpContext, Exception exception)
    {
      if (hostEnvironment.IsDevelopment())
      {
        //TODO complate this
        await httpContext.Response.WriteAsync(exception.Message.ToString());
      }
      else
        await httpContext.Response.WriteAsync(exception.Message.ToString());
    }
  }
}