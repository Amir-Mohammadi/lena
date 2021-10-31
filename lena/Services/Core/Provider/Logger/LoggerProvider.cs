using lena.Domains.Enums;
using lena.Models.ApplicationBase.Logger;
using lena.Models.UserManagement.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Logger
{
  public abstract class LoggerProvider : Provider
  {

    public abstract string New(string secret, string owner);

    public abstract void Remove(string id, string owner);

    public abstract void Log(Log log);


    public abstract void Clear();


    public void Debug(string payload)
    {
#if DEBUG
      try
      {
        var requestProvider = App.Providers.Request;
        var log = new Log()
        {
          Method = requestProvider.Method,
          RemoteAddress = requestProvider.ClientAddress,
          SessionId = App.Providers.Session.StateKey,
          Type = LogType.Debug,
          Url = requestProvider.Url,
          Parameters = requestProvider.Parameters,
          Login = App.Providers.Security.CurrentLoginData,
          Payload = payload
        };
        App.Providers.Logger.Log(log);
      }
      catch (Exception) { }
#endif
    }

    public void Info(string payload)
    {
      try
      {
        var requestProvider = App.Providers.Request;
        var log = new Log()
        {
          SessionId = App.Providers.Session.StateKey,
          Method = requestProvider.Method,
          RemoteAddress = requestProvider.ClientAddress,
          Type = LogType.Info,
          Url = requestProvider.Url,
          Parameters = requestProvider.Parameters,
          Login = App.Providers.Security.CurrentLoginData,
          Payload = payload
        };
        App.Providers.Logger.Log(log);
      }
      catch (Exception) { }
    }


    public void Error(string stackTrace, string payload)
    {
      try
      {

        var requestProvider = App.Providers.Request;
        var log = new Log()
        {
          Method = requestProvider.Method,
          RemoteAddress = requestProvider.ClientAddress,
          SessionId = App.Providers.Session.StateKey,
          Type = LogType.Error,
          Url = requestProvider.Url,
          Parameters = requestProvider.Parameters,
          Login = App.Providers.Security.CurrentLoginData,
          Payload = payload,
          StackTrace = stackTrace
        };
        App.Providers.Logger.Log(log);
      }
      catch (Exception) { }
    }

    public void Warning(string payload)
    {
      try
      {
        var requestProvider = App.Providers.Request;
        var log = new Log()
        {
          Method = requestProvider.Method,
          RemoteAddress = requestProvider.ClientAddress,
          SessionId = App.Providers.Session.StateKey,
          Type = LogType.Warning,
          Url = requestProvider.Url,
          Parameters = requestProvider.Parameters,
          Login = App.Providers.Security.CurrentLoginData,
          Payload = payload
        };
        App.Providers.Logger.Log(log);

      }
      catch (Exception) { }
    }


  }
}
