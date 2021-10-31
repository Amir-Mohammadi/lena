using System;
using Microsoft.AspNetCore.Http;
using core.Autofac;
using System.Threading.Tasks;
namespace core.ExceptionHandler
{
  public interface IExceptionHandlerService : IScopedDependency
  {
    Task Handle(HttpContext httpContext, Exception exception);
  }
}