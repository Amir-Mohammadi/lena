using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Calendar
{
  public class AddCalendarInput
  {
    public DateTime Date { get; set; }
    public bool IsWorkingDay { get; set; }
    public bool IsHoliday { get; set; }
  }
}
