using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Logger
{
  public class PersistentLogger : Provider
  {

    public const int FLORA_ROUTE_BLOCKED = -2;
    public const int FLORA_SERVER_IS_NOT_RESPONDING = -1;
    public const int FLORA_CLIENT_IS_DISABLED = -3;


    private readonly ILogger logger;
    private PersistentLog log = new PersistentLog("");

    public List<PersistentLogStep> Steps => log.Steps;
    public PersistentLog Log => log;

    public object Request;

    public object Response;

    public PersistentLogger(ILogger logger)
    {
      this.logger = logger;

      try
      {
        var requestProvider = App.Providers.Request;
        var persistentLog = new PersistentLog(requestProvider.Url);
        this.Request = new
        {
          Url = requestProvider.Url,
          ClientIp = requestProvider.ClientAddress,
          Method = requestProvider.Method,
          Params = requestProvider.Parameters,
          Session = App.Providers.Security.CurrentLoginData
        };
        this.Start(persistentLog);
      }
      catch (Exception)
      {

      }
    }

    public void Start(PersistentLog log)
    {
      this.log = log;
    }

    public int Finish()
    {
      if (!App.Providers.Storage.PersistentLoggerEnabled)
      {
        return FLORA_CLIENT_IS_DISABLED;
      }
      try
      {
        if (App.Providers.Storage.PersistentLoggerRouteBlackList.Contains(App.Providers.Request.Url?.ToLower()))
        {
          return FLORA_ROUTE_BLOCKED;
        }
        log.RawValue = new
        {
          Header = Request,
          Timer = App.Providers.Request.Timer.Elapsed.Milliseconds
        };
        return this.logger.Log(log);
      }
      catch (Exception)
      {
        return FLORA_SERVER_IS_NOT_RESPONDING;
      }
    }


  }
}
