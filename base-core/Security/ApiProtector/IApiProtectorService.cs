using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using core.Autofac;

namespace core.Security.ApiProtector
{
  public interface IApiProtectorService : ISingletonDependency
  {
    Task Protect(HttpContext context);
  }
}