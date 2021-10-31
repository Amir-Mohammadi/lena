using System;

using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class CheckClientTimeInfoResult
  {
    public DateTime DateTime { get; set; }
    public string TimeZoneId { get; set; }
    public string TimeZoneName { get; set; }
  }
}
