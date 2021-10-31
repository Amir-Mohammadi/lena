using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Calendar
{
  public class CalendarResult
  {
    public DateTime Date { get; set; }
    public int DayTypeId { get; set; }
    public bool IsWorkingDay { get; set; }
    public bool IsHoliday { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
