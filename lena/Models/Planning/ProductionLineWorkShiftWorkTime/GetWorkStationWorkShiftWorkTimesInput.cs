using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineWorkShiftWorkTime
{
  public class GetProductionLineWorkShiftWorkTimesInput
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
  }
}
