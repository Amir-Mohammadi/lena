using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineWorkShift
{
  public class ProductionLineWorkShiftResult
  {
    public int Id { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public int WorkShiftId { get; set; }
    public string WorkShiftName { get; set; }
    public DateTime SaveDate { get; set; }
    public DateTime FromDate { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
