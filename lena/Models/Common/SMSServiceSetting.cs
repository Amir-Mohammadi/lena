using System.Collections.Generic;
using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class SMSServiceSetting
  {
    public string SourceNumber { get; set; }
    public string Url { get; set; }
    public string ApiKey { get; set; }
  }
}