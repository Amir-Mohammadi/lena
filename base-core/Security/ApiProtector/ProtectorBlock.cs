using System;

namespace core.Security.ApiProtector
{
  public class ProtectorBlock
  {
    public string ClientIp { get; set; }
    public string Identity { get; set; }
    public int? AppRole { get; set; }
    public string Url { get; set; }
    public DateTime LockoutDateTime { get; set; }
  }
}