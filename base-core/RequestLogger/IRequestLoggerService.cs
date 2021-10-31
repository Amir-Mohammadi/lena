using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using core.Autofac;
using System;
namespace core.RequestLogger
{
  public interface IRequestLoggerService : ISingletonDependency
  {
    IEnumerable<Log> GetLogs();
    Task Log(HttpContext context, Exception exception = null);
  }
}