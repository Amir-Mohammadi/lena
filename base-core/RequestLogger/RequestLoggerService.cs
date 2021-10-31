using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
namespace core.RequestLogger
{
  public class RequestLoggerService : IRequestLoggerService
  {
    readonly List<Log> logs = new();
    private async Task AddLog(Log log)
    {
      logs.Add(log);
    }
    public async Task Log(HttpContext context, Exception exception = null)
    {
      var clientIp = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
      var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
      var timeSpan = new TimeSpan(100);
      if (IsExclusionsRequest(context.Response?.StatusCode))
        return;
      //ToDo fix it 
      string identity = null;
      int? appRole = null;
      var log = new Log()
      {
        Identity = identity,
        ClientIp = clientIp,
        AppRole = appRole,
        EndPointUrl = endpoint.DisplayName,
        DateTime = DateTime.UtcNow,
        TimeSpan = timeSpan,
        Exception = exception
      };
      await AddLog(log: log);
    }
    public IEnumerable<Log> GetLogs()
    {
      return logs;
    }
    private bool IsExclusionsRequest(int? statusCode)
    {
      //ToDo complate this
      return false;
    }
  }
}