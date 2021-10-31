using lena.Domains.Enums;
namespace lena.Models.Planning.WorkTime
{
  public class AddDayWorkTimeInput
  {
    public long FromTime { get; set; }
    public long Duration { get; set; }
    public int WorkShiftId { get; set; }
    public int DayNumber { get; set; }
  }
}
