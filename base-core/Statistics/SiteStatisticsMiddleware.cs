using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace core.Statistics
{
  public class StatisticsMiddleware
  {
    #region Fields
    private readonly RequestDelegate next;
    private readonly IStatisticsService statisticsService;
    #endregion
    #region Constractor
    public StatisticsMiddleware(RequestDelegate next,
                                IStatisticsService statisticsService)
    {
      this.next = next;
      this.statisticsService = statisticsService;
    }
    #endregion
    #region Invoke
    public async Task Invoke(HttpContext context)
    {
      await next(context);
      await this.statisticsService.Apply(context);
    }
    #endregion
  }
}