using lena.Domains.Enums;
using lena.Models.UserManagement.User;
using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Logger
{
  public class Log
  {
    public string Url { get; set; }

    public int Date { get; set; } = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    public string SessionId { get; set; } = "";
    public object Parameters { get; set; }
    public LogType Type { get; set; }
    public LoginResult Login { get; set; }

    public string Method { get; set; }

    public string RemoteAddress { get; set; }
    public string Payload { get; set; }

    public string StackTrace { get; set; } = "";

  }
}
