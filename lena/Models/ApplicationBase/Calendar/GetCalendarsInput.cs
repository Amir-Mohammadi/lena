using System;
using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Calendar
{
  public class GetCalendarsInput
  {
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
  }
}
