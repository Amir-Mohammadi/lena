using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using core.RequestLogger;
using core.ExceptionHandler;
namespace core.Security.ApiProtector
{
  public class ApiProtectorService : IApiProtectorService
  {
    private readonly IErrorFactory errorFactory;
    private readonly IRequestLoggerService requestLoggerService;
    private readonly List<ProtectorBlock> blocks = new();
    public ApiProtectorService(IErrorFactory errorFactory,
                               IRequestLoggerService requestLoggerService)
    {
      this.errorFactory = errorFactory;
      this.requestLoggerService = requestLoggerService;
    }
    public async Task Protect(HttpContext context)
    {
      var clientIp = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
      if (IsExclusionsIP(clientIp))
        return;
      var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
      var apiProtectorConditions = endpoint?.Metadata.GetOrderedMetadata<ApiProtectorAttribute>().Distinct();
      if (apiProtectorConditions == null || !apiProtectorConditions.Any())
        return;
      //ToDo fix it 
      string identity = null;
      int? appRole = null;
      var url = endpoint.DisplayName;
      await CheckBlock(identity: identity, clientIp: clientIp, appRole: appRole, url: url);
      var logs = requestLoggerService.GetLogs()
                                     .Where(x => x.EndPointUrl == url);
      foreach (var condition in apiProtectorConditions)
      {
        switch (condition.ApiProtectionType)
        {
          case ApiProtectionType.ByIdentity:
            logs = logs.Where(x => x.Identity == identity);
            break;
          case ApiProtectionType.ByIpAddress:
            logs = logs.Where(x => x.ClientIp == clientIp);
            break;
          case ApiProtectionType.ByRole:
            logs = logs.Where(x => x.AppRole == appRole);
            break;
        }
        var checkDateTime = DateTime.UtcNow.AddSeconds(-1 * condition.TimeWindowSecond);
        logs = logs.Where(x => x.DateTime >= checkDateTime);
        if (logs.Count() >= condition.Limit)
        {
          var lockoutDateTime = logs.Min(x => x.DateTime);
          lockoutDateTime = lockoutDateTime.AddSeconds(condition.TimeWindowSecond + condition.PenaltySecond);
          await AddBlock(clientIp: clientIp,
                         identity: identity,
                         appRole: appRole,
                         url: url,
                         lockoutDateTime: lockoutDateTime);
          throw errorFactory.ApiProtectorAttackDetect();
        }
      }
    }
    private Task AddBlock(string clientIp, string identity, int? appRole, string url, DateTime lockoutDateTime)
    {
      var block = new ProtectorBlock()
      {
        ClientIp = clientIp,
        Identity = identity,
        AppRole = appRole,
        Url = url,
        LockoutDateTime = lockoutDateTime
      };
      blocks.Add(block);
      return Task.CompletedTask;
    }
    private Task CheckBlock(string clientIp, string identity, int? appRole, string url)
    {
      var query = blocks.Where(x => x.Url == url && x.LockoutDateTime > DateTime.UtcNow);
      query = query.Where(x => x.ClientIp == clientIp
                               || (identity != null && x.Identity == identity)
                               || (appRole != null && x.AppRole == appRole));
      if (query.Any())
        throw errorFactory.ApiProtectorAttackDetect();
      return Task.CompletedTask;
    }
    private bool IsExclusionsIP(string clientIp)
    {
      return false;
    }
  }
}