using System.Threading.Tasks;
using core.Autofac;
using Microsoft.AspNetCore.Http;
namespace core.HttpStatusCodeHandler
{
  public interface IHttpStatusCodeService : ISingletonDependency
  {
    Task Apply(HttpContext context);
  }
}