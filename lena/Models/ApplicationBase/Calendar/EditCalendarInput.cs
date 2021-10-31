using System;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Calendar
{
  public class EditCalendarInput
  {
    public DateTime Date { get; set; }
    public TValue<bool> IsWorkingDay { get; set; }
    public TValue<bool> IsHoliday { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
