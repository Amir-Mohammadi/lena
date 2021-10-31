using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkTime
{
  public class AddDayWorkTimesInput
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int Period { get; set; }
    public bool OnlyApplyToWorkDays { get; set; }
    public AddDayWorkTimeInput[] DayWorkTimeInputs { get; set; }
  }
}
