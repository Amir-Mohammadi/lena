using System;

namespace core.RequestLogger
{
  public class Log
  {
    public DateTime DateTime { get; set; }
    public string ClientIp { get; set; }
    public string EndPointUrl { get; set; }
    public int? AppRole { get; set; }
    public string Identity { get; set; }
    public TimeSpan TimeSpan { get; set; }
    public Exception Exception { get; set; }
  }
}