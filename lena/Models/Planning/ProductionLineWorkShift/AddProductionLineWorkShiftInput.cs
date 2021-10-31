using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineWorkShift
{
  public class AddProductionLineWorkShiftInput
  {
    public int ProductionLineId { get; set; }
    public int WorkShiftId { get; set; }
    public DateTime SaveDate { get; set; }
    public DateTime FromDate { get; set; }
  }
}
