using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkTime
{
  public class AddWeekWorkTimesInput
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool OnlyApplyToWorkDays { get; set; }
    public AddWeekWorkTimeInput[] WeekWorkTimeInputs { get; set; }

  }
}
