using System.Threading.Tasks;
using core.Autofac;
using Microsoft.AspNetCore.Http;
namespace core.Statistics
{
  public interface IStatisticsService : ISingletonDependency
  {
    Task Apply(HttpContext context);
  }
}