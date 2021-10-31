using Microsoft.AspNetCore.Builder;
using core.ExceptionHandler;
using core.FileHandler;
using core.Transaction;
using core.Statistics;
using core.HttpStatusCodeHandler;
using core.RequestLogger;
using core.Security.ApiProtector;
namespace core.Extensions
{
  public static class MiddlewareExtensions
  {
    public static IApplicationBuilder UseTransactionsPerRequest(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<TransactionMiddleware>();
    }
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
    public static IApplicationBuilder UseFileHandler(this IApplicationBuilder builder)
    {
      return builder.MapWhen(
        predicate: context => context.Request.Path.ToString().Contains("document.ashx"),
        configuration: appBranch =>
        {
          appBranch.UseMiddleware<FileHandlerMiddleware>();
        });
    }
    public static IApplicationBuilder UseHttpStatusCode(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<HttpStatusCodeMiddleware>();
    }
    public static IApplicationBuilder UseStatistics(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<StatisticsMiddleware>();
    }
    public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<RequestLoggerMiddleware>();
    }
    public static IApplicationBuilder UseApiProtector(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ApiProtectorMiddleware>();
    }
  }
}