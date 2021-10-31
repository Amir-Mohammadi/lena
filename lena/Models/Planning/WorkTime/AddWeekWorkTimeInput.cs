using System;
using lena.Domains.Enums;
namespace lena.Models.Planning.WorkTime
{
  public class AddWeekWorkTimeInput
  {
    public long FromTime { get; set; }
    public long Duration { get; set; }
    public int WorkShiftId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
  }
}
