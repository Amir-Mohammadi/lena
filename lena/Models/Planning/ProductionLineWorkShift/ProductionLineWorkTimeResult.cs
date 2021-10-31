using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineWorkShift
{
  public class ProductionLineWorkTimeResult
  {
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public string WorkShiftName { get; set; }
    public int WorkShiftId { get; set; }
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public DateTime ToDateTime => DateTime.AddSeconds(Duration);
    public byte[] RowVersion { get; set; }
  }
}
