using System;

using lena.Domains.Enums;
namespace lena.Models.Application
{
  public class AddApplicationLogInput
  {
    public int? UserId { get; set; }
    public string ClientIp { get; set; }
    public string UserAgent { get; set; }
    public string Action { get; set; }
    public DateTime? RequestEndTime { get; set; }
  }
}
