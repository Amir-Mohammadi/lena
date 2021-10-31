using System;

using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class CheckClientTimeInfoInput
  {
    public DateTime DateTime { get; set; }
    public int TimeZoneOffsetInMinutes { get; set; }
  }
}
