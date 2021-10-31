using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkTime
{
  public class WorkTimeResult
  {
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public DateTime ToDateTime => DateTime.AddSeconds(Duration);
    public int? WorkShiftId { get; set; }
    public string WorkShiftName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
