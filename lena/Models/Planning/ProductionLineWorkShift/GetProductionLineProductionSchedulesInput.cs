using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineWorkShift
{
  public class GetProductionLineProductionSchedulesInput
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
  }
}
