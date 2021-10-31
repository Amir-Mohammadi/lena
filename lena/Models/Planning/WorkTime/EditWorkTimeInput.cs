using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkTime
{
  public class EditWorkTimeInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public long FromTime { get; set; }
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public int WorkShiftId { get; set; }
  }
}
